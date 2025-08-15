using fiotec_Serpro.Domain.Exceptions;
using fiotec_Serpro.Infra.Services.Interfaces;
using fiotec_Serpro.Infra.Services.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace fiotec_Serpro.Infra.Services.Services
{
    public class SerproService(IConfiguration _configuration, IHttpClientFactory _httpClient) : ISerproService
    {
        public async Task<RetornoDataValid?> ValidaCPFSerpro(string cpf)
        {
            var token = await GetTokenSerproAsync();
            Log.Information("Obtendo token Serpro para CPF {Cpf}", cpf);

            if (token == null || string.IsNullOrEmpty(token.access_token))
            {
                Log.Error("Não foi possível obter token do SERPRO.");
                throw new TokenInvalidoException("Não foi possível obter token do SERPRO.");
            }

            var payload = new
            {
                key = new { cpf },
                answer = new
                {
                    nome = "",
                    sexo = "",
                    data_nascimento = "",
                    situacao_cpf = "",
                    filiacao = new { nome_mae = "", nome_pai = "" },
                    nacionalidade = 1,
                    endereco = new { logradouro = "", numero = "", complemento = "", bairro = "", cep = "", municipio = "", uf = "" },
                    documento = new { tipo = 1, numero = "", orgao_expedidor = "", uf_expedidor = "" },
                    cnh = new { numero_registro = "", registro_nacional_estrangeiro = "", categoria = "", data_primeira_habilitacao = "", data_validade = "" }
                }
            };

            Log.Information("Payload enviado para o SERPRO: {@Payload}", payload);

            var client = _httpClient.CreateClient("SerproApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var endpoint = _configuration["Serpro:dataValidEndpoint"];

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(endpoint, content);
                Log.Information("Requisição enviada para {Endpoint}", endpoint);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao tentar se conectar ao SERPRO para CPF {Cpf}", cpf);
                throw new ConexaoSerproException("Erro ao tentar se conectar ao SERPRO.", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                Log.Warning("Serviço do SERPRO retornou status {StatusCode} para CPF {Cpf}", response.StatusCode, cpf);
                throw new ServicoIndisponivelException($"Serviço do SERPRO retornou status {response.StatusCode}.");
            }

            var result = await response.Content.ReadAsStringAsync();
            Log.Information("Resposta recebida do SERPRO para CPF {Cpf}: {Response}", cpf, result);

            return JsonSerializer.Deserialize<RetornoDataValid>(result);
        }

        private async Task<RetornoToken?> GetTokenSerproAsync()
        {
            var client = _httpClient.CreateClient("SerproAuth");

            var chaveBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_configuration["Serpro:key"]));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", chaveBase64);

            var parameters = new Dictionary<string, string> { { "grant_type", "client_credentials" } };

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync("/token", new FormUrlEncodedContent(parameters));
                Log.Information("Requisição de token enviada ao SERPRO");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao tentar obter token do SERPRO");
                throw new TokenInvalidoException("Não foi possível se conectar ao SERPRO para obter o token.", ex);
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Log.Information("Token recebido com sucesso");

                return JsonSerializer.Deserialize<RetornoToken>(json);
            }

            Log.Warning("Falha ao obter token do SERPRO. StatusCode: {StatusCode}", response.StatusCode);
            throw new TokenInvalidoException($"Falha ao obter token do SERPRO. StatusCode: {response.StatusCode}");
        }
    }
}