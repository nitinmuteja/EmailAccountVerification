using ProcessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    interface IEmailAccountVerificationProcess
    {
        ValidateAccountResponse ValidateAccount(string EmailAddress);



    }
}
