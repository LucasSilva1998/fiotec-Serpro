using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Infra.Services.Models
{
    public class RetornoToken
    {
        public string? scope { get; set; }
        public string? token_type { get; set; }
        public int expires_in { get; set; }
        public string? access_token { get; set; }
    }
}