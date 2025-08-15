using fiotec_Serpro.Domain.Exceptions;
using fiotec_Serpro.Infra.Services.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace fiotec_Serpro.Infra.Tests.Unit.Services
{
    public class SerproServiceTests
    {
        private SerproService CriarServicoComMocks(out Mock<HttpMessageHandler> authHandlerMock,
                                                  out Mock<HttpMessageHandler> apiHandlerMock)
        {
            // Mock para Auth
            authHandlerMock = new Mock<HttpMessageHandler>();
            var authClient = new HttpClient(authHandlerMock.Object)
            {
                BaseAddress = new Uri("https://gateway.apiserpro.serpro.gov.br")
            };

            // Mock para API
            apiHandlerMock = new Mock<HttpMessageHandler>();
            var apiClient = new HttpClient(apiHandlerMock.Object)
            {
                BaseAddress = new Uri("https://gateway.apiserpro.serpro.gov.br")
            };

            // Mock IHttpClientFactory
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(f => f.CreateClient("SerproAuth")).Returns(authClient);
            httpFactoryMock.Setup(f => f.CreateClient("SerproApi")).Returns(apiClient);

            // Mock IConfiguration
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Serpro:key"]).Returns("chave_teste");
            configMock.Setup(c => c["Serpro:dataValidEndpoint"]).Returns("/datavalid/v2/validate/pf");

            return new SerproService(configMock.Object, httpFactoryMock.Object);
        }

        [Fact]
        public async Task ValidaCPFSerpro_DeveRetornarDados_QuandoTudoOk()
        {
            var servico = CriarServicoComMocks(out var authHandler, out var apiHandler);

            // Mock do token
            var tokenJson = @"{""access_token"":""token_teste""}";
            authHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(tokenJson, Encoding.UTF8, "application/json")
                });

            // Mock da API de validação
            var retornoJson = @"{
                ""cpf_disponivel"": true,
                ""filiacao"": { ""nome_mae"": ""Maria"", ""nome_pai"": ""João"" },
                ""endereco"": { ""logradouro"": ""Rua A"", ""numero"": ""123"" },
                ""documento"": { ""tipo"": ""CPF"", ""numero"": ""12345678909"" },
                ""cnh"": { ""numero_registro"": ""123456789"" },
                ""nacionalidade"": true
            }";

            apiHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(retornoJson, Encoding.UTF8, "application/json")
                });

            var resultado = await servico.ValidaCPFSerpro("12345678909");

            Assert.NotNull(resultado);
            Assert.True(resultado.cpf_disponivel);

            // Usando JsonElement para acessar propriedades internas
            var filiacao = (JsonElement)resultado.filiacao!;
            var endereco = (JsonElement)resultado.endereco!;
            var documento = (JsonElement)resultado.documento!;

            Assert.Equal("Maria", filiacao.GetProperty("nome_mae").GetString());
            Assert.Equal("João", filiacao.GetProperty("nome_pai").GetString());

            Assert.Equal("Rua A", endereco.GetProperty("logradouro").GetString());
            Assert.Equal("123", endereco.GetProperty("numero").GetString());

            Assert.Equal("12345678909", documento.GetProperty("numero").GetString());
        }

        [Fact]
        public async Task ValidaCPFSerpro_DeveLancarServicoIndisponivelException_QuandoTokenNull()
        {
            var servico = CriarServicoComMocks(out var authHandler, out var apiHandler);

            // Token retorna BadRequest
            authHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

            await Assert.ThrowsAsync<ServicoIndisponivelException>(() =>
                servico.ValidaCPFSerpro("12345678909")
            );
        }

        [Fact]
        public async Task ValidaCPFSerpro_DeveLancarServicoIndisponivelException_QuandoHttpFalhaNaApi()
        {
            var servico = CriarServicoComMocks(out var authHandler, out var apiHandler);

            // Token válido
            var tokenJson = @"{""access_token"":""token_teste""}";
            authHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(tokenJson, Encoding.UTF8, "application/json")
                });

            apiHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Falha de conexão"));

            await Assert.ThrowsAsync<ServicoIndisponivelException>(() =>
                servico.ValidaCPFSerpro("12345678909")
            );
        }
    }
}
