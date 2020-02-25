using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRMApi.Models
{
    public class TokenInput
    {
        public TokenInput()
        {

        }
        public string Grant_Type { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
