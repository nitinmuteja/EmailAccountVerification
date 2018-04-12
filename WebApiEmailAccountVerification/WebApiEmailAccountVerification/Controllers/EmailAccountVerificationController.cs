using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Process;
using ProcessModel;
using ServiceModel;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebApiEmailAccountVerification.Controllers
{
    
    public class EmailAccountVerificationController : ApiController
    {


        [HttpGet]
      //[Route("api/EmailAccountVerification/{Emailaddress}")]
        [ResponseType(typeof(ApiResponse))]
        // GET api/values
        public async Task<IHttpActionResult> Get(String Emailaddress)
        {
            try
            {
                if(!IsEmailValid(Emailaddress))
                {
                    return BadRequest("InvalidEmailAddress");
                }
                EmailAccountVerificationProcess process = new EmailAccountVerificationProcess();
                ValidateAccountResponse Response = process.ValidateAccount(Emailaddress);
                return Ok( new ApiResponse() { Result = Response.Result, VerificationResult = Response.VerificationResult, ErrorCode = Response.ErrorCode });
            }
            catch (Exception ex)
            {
              return  InternalServerError(ex);

            }
         }



        private bool IsEmailValid(string value)
        {
            if (value == null)
            {
                return true;
            }
            string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            Regex regex = new Regex(emailRegex);
            if (regex.IsMatch(value.ToLower()))
                return true;
            else
                return false;
        }



    }
}
