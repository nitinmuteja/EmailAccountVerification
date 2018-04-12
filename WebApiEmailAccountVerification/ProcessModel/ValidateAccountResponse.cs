using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessModel
{
    public class ValidateAccountResponse
    {
        public bool VerificationResult { get; set; }
        public bool Result { get; set; }
        public string ErrorCode { get; set; }

    }
}
