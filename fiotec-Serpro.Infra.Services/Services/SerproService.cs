using fiotec_Serpro.Domain.Exceptions;
using fiotec_Serpro.Infra.Services.Interfaces;
using fiotec_Serpro.Infra.Services.Models;
using Microsoft.Extensions.Configuration;
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
            if (token == null || string.IsNullOrEmpty(token.access_token))
                throw new ServicoIndisponivelException("Não foi possível obter token do SERPRO.");

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

            var client = _httpClient.CreateClient("SerproApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var endpoint = _configuration["Serpro:dataValidEndpoint"];

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(endpoint, content);
            }
            catch (Exception ex)
            {
                throw new ServicoIndisponivelException("Erro ao tentar se conectar ao SERPRO.", ex);
            }

            if (!response.IsSuccessStatusCode)
                throw new ServicoIndisponivelException($"Serviço do SERPRO retornou status {response.StatusCode}.");

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RetornoDataValid>(result);
        }

        private async Task<RetornoToken?> GetTokenSerproAsync()
        {
            var client = _httpClient.CreateClient("SerproAuth");

            var chaveBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_configuration["Serpro:key"]));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", chaveBase64);

            var parameters = new Dictionary<string, string> { { "grant_type", "client_credentials" } };
            var response = await client.PostAsync("/token", new FormUrlEncodedContent(parameters));

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RetornoToken>(json);
            }

            return null;
        }
    }
}