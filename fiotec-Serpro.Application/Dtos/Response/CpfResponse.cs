using fiotec_Serpro.Infra.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Application.Dtos.Response
{
    public class CpfResponse
    {
        public bool CpfDisponivel { get; set; }
        public object Filiacao { get; set; }
        public object Endereco { get; set; }
        public object Documento { get; set; }
        public object Cnh { get; set; }
        public bool Nacionalidade { get; set; }

        public CpfResponse(RetornoDataValid data)
        {
            CpfDisponivel = data.cpf_disponivel;
            Filiacao = data.filiacao;
            Endereco = data.endereco;
            Documento = data.documento;
            Cnh = data.cnh;
            Nacionalidade = data.nacionalidade;
        }
    }
}
