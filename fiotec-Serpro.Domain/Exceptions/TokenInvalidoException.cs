using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Domain.Exceptions
{
    public class TokenInvalidoException : Exception
    {
        public TokenInvalidoException(string message) : base(message) { }
        public TokenInvalidoException(string message, Exception inner) : base(message, inner) { }
    }
}