using fiotec_Serpro.Application.Dtos.Response;
using fiotec_Serpro.Application.Interfaces;
using fiotec_Serpro.Domain.Exceptions;
using fiotec_Serpro.Domain.ValueObjects;
using fiotec_Serpro.Infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Application.Services
{
    public class SerproAppService(ISerproService serproService) : ISerproAppService
    {    
        public async Task<CpfResponse> ValidarCpfAsync(string cpfInput)
        {
            Cpf cpf;

            try
            {
                cpf = new Cpf(cpfInput);
            }
            catch (Exception ex)
            {
                throw new CpfInvalidoException(ex.Message, ex);
            }

            var resultado = await serproService.ValidaCPFSerpro(cpf.Numero);

            if (resultado == null)
                throw new ServicoIndisponivelException();

            return new CpfResponse(resultado);
        }
    }
}