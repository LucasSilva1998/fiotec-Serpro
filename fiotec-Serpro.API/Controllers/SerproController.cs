using fiotec_Serpro.Application.Dtos.Response;
using fiotec_Serpro.Application.Interfaces;
using fiotec_Serpro.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fiotec_Serpro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SerproController(ISerproAppService serproAppService) : ControllerBase
    {
        /// <summary>
        /// Consulta CPF no serviço SERPRO
        /// </summary>
        /// <param name="cpf">Número do CPF a ser consultado</param>
        /// <param name="ct">CancellationToken opcional</param>
        /// <returns>Dados do CPF ou erro padronizado</returns>
        [HttpGet("validar-cpf/{cpf}")]
        [ProducesResponseType(typeof(CpfResponse), 200)]
        [ProducesResponseType(400)] // CPF inválido
        [ProducesResponseType(503)] // Serviço indisponível
        [ProducesResponseType(500)] // Erro interno
        public async Task<IActionResult> ValidarCpf(string cpf, CancellationToken ct = default)
        {
            var resultado = await serproAppService.ValidarCpfAsync(cpf);
            return Ok(resultado);
        }
    }
}