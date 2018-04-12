using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Process;
using ProcessModel;
using System.Threading;

namespace TestProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
           
            ValidateAccountResponse result;
            EmailAccountVerificationProcess obj = new EmailAccountVerificationProcess();
            result=obj.ValidateAccount("nansee.chauhan@bold.com");
            Console.WriteLine("API Response "+result.Result+" Account Verified "+result.VerificationResult+" Error "+result.ErrorCode);
            result = obj.ValidateAccount("nitin.muteja@bold.com");
            Console.WriteLine("API Response " + result.Result + " Account Verified " + result.VerificationResult + " Error " + result.ErrorCode);
            result = obj.ValidateAccount("nitinmuteja@gmail.com");
            Console.WriteLine("API Response " + result.Result + " Account Verified " + result.VerificationResult + " Error " + result.ErrorCode);
            result = obj.ValidateAccount("nitinmutejahdjhdjd@gmail.com");
            Console.WriteLine("API Response " + result.Result + " Account Verified " + result.VerificationResult + " Error " + result.ErrorCode);
            result = obj.ValidateAccount("nitinmuteja@yahoo.com");
            Console.WriteLine("API Response " + result.Result + " Account Verified " + result.VerificationResult + " Error " + result.ErrorCode);
            result = obj.ValidateAccount("nansee.chauhan@nitinmuteja.com");
            Console.WriteLine("API Response " + result.Result + " Account Verified " + result.VerificationResult + " Error " + result.ErrorCode);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("TimeElapsed " + elapsedMs);

            Console.ReadLine();

        }
    }
}
