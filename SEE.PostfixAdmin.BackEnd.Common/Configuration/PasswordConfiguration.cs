using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Configuration
{
    public class PasswordConfiguration
    {
        public string Encoder { get; set; } = "SHA1";
        public int MinLength { get; set; } = 8;
        public bool BigLetter { get; set; } = false;
        public bool SpecialChar { get; set; } = false;
        public bool Digit { get; set; } = false;
    }
}
