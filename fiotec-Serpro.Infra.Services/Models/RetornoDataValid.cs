using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Infra.Services.Models
{
    public class RetornoDataValid
    {
        public bool nacionalidade { get; set; }
        public object? filiacao { get; set; }
        public object? endereco { get; set; }
        public object? cnh { get; set; }
        public object? documento { get; set; }
        public bool cpf_disponivel { get; set; }

    }
}