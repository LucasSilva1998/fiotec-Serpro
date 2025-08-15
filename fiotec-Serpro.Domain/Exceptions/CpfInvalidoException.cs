using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Domain.Exceptions
{
    public class CpfInvalidoException : Exception
    {
        public CpfInvalidoException() : base("CPF informado é inválido.") { }

        public CpfInvalidoException(string message) : base(message) { }

        public CpfInvalidoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}