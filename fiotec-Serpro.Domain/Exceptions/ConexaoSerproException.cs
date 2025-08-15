using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Domain.Exceptions
{
    public class ConexaoSerproException : Exception
    {
        public ConexaoSerproException(string message) : base(message) { }
        public ConexaoSerproException(string message, Exception inner) : base(message, inner) { }
    }
}