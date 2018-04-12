using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ProcessModel;
namespace Process
{
    public class EmailAccountVerificationProcess : IEmailAccountVerificationProcess
    {
        public ValidateAccountResponse ValidateAccount(string EmailAddress)
        {
            ValidateAccountResponse Result = new ValidateAccountResponse() { Result = true, VerificationResult = false, ErrorCode = "" };
try
            {
                string receiptTo = EmailAddress;
                string command = "nslookup -type=MX ";
                string response = "";
                String[] arrSplit = receiptTo.Split('@');
                string[] mxUrl = null;
                string MailExchanger = "mail exchanger =";
                string domain = arrSplit[arrSplit.Length-1];
                command += domain;

                ExecuteCommandSync(command, ref response);
                

                foreach (String line in response.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {


                    if (line.Contains(domain))
                    {

                        string tempUrl = line.IndexOf("mail exchanger =") < 0 ? null : line.Substring(line.IndexOf(MailExchanger) + MailExchanger.Length).Trim();

                        if (tempUrl != null)
                        {
                            if (mxUrl == null)
                            {
                                mxUrl = new string[1];
                            }
                            else
                            {
                                Array.Resize<string>(ref mxUrl, mxUrl.Length + 1);
                            }
                            mxUrl[mxUrl.Length - 1] = tempUrl;
                        }
                    }

                }

                //mx servers found if mxUrl!=null
                if (mxUrl != null)
                {
                    bool accountFound = false;
                    for (int i = 0; i < mxUrl.Length; i++)
                    {
                        //smtp servers run on either of 25,465,587 ports
                        TcpClient tClient;
                        try
                        {
                            tClient = new TcpClient(mxUrl[i], 25);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                tClient = new TcpClient(mxUrl[i], 465);
                            }
                            catch (Exception ex2)
                            {
                                tClient = new TcpClient(mxUrl[i], 587);
                            }
                        }


                        string CRLF = "\r\n";
                        byte[] dataBuffer;
                        string ResponseString;
                        NetworkStream netStream = tClient.GetStream();
                        StreamReader reader = new StreamReader(netStream);
                        ResponseString = reader.ReadLine();
                        /* Perform HELO to SMTP Server and get Response */
                        dataBuffer = BytesFromString("HELO " + domain + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        ResponseString = reader.ReadLine();
                        dataBuffer = BytesFromString("MAIL FROM:<nitin.muteja@bold.com>" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        ResponseString = reader.ReadLine();
                        /* Read Response of the RCPT TO Message to know from SMTP if it exist or not */
                        dataBuffer = BytesFromString("RCPT TO:<" + receiptTo + ">" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        ResponseString = reader.ReadLine();
                        if (GetResponseCode(ResponseString) == 250)
                        {
                            accountFound = true;
                        }
                        /* QUIT CONNECTION */
                        dataBuffer = BytesFromString("QUIT" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        ResponseString = reader.ReadLine();
                        tClient.Close();


                        if (accountFound)
                            break;

                    }

                    Result.VerificationResult= accountFound;
                }
                else
                {
                    Result.VerificationResult=false;
                    Result.ErrorCode = "No MX servers Found";
                }
            }
            catch (Exception ex)
            {
                Result.VerificationResult = false;
                Result.Result = false;
                Result.ErrorCode = ex.Message;
            }

            return Result;
        }


        private static int GetResponseCode(string ResponseString)
        {
            return int.Parse(ResponseString.Substring(0, 3));
        }


        private static byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string ExecuteCommandSync(object command, ref string Result)
        {   
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string resultConsole = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Result = resultConsole;
                return "SUCCESS";       
        }



    }




}
