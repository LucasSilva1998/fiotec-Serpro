using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Domain.Exceptions
{
    public class ServicoIndisponivelException : Exception
    {
        public ServicoIndisponivelException()
            : base("Serviço externo indisponível.") { }

        public ServicoIndisponivelException(string message) : base(message) { }

        public ServicoIndisponivelException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}