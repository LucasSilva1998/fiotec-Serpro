using fiotec_Serpro.Infra.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Infra.Services.Interfaces
{
    public interface ISerproService
    {
        Task<RetornoDataValid> ValidaCPFSerpro(string cpf);
    }
}