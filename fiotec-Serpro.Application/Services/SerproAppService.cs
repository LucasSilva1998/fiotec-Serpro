using fiotec_Serpro.Application.Dtos.Response;
using fiotec_Serpro.Application.Interfaces;
using fiotec_Serpro.Domain.Exceptions;
using fiotec_Serpro.Domain.ValueObjects;
using fiotec_Serpro.Infra.Services.Interfaces;
using Serilog;
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
            Log.Information("Iniciando validação de CPF na Application: {CpfInput}", cpfInput);

            Cpf cpf;

            try
            {
                cpf = new Cpf(cpfInput);
                Log.Information("CPF válido: {Cpf}", cpf.Numero);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CPF inválido: {CpfInput}", cpfInput);
                throw new CpfInvalidoException(ex.Message, ex);
            }

            var resultado = await serproService.ValidaCPFSerpro(cpf.Numero);
            if (resultado == null)
            {
                Log.Error("Serviço do SERPRO retornou nulo para o CPF {Cpf}", cpf.Numero);
                throw new ServicoIndisponivelException();
            }

            Log.Information("Validação do CPF concluída com sucesso: {Cpf}", cpf.Numero);
            return new CpfResponse(resultado);
        }
    }
}