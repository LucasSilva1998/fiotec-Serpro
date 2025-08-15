using fiotec_Serpro.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Application.Interfaces
{
    public interface ISerproAppService
    {
        Task<CpfResponse> ValidarCpfAsync(string cpf);
    }
}