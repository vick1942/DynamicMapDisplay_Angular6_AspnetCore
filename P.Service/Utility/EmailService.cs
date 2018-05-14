using Common.Constants;
using NLog;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace P.Service.Utility
{
    public class EmailService
    {
        static Logger loggers = LogManager.GetLogger(Constants.PServiceLogger);
        public static void SendEmail(string subject, string message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["MailHost"]);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["From"]);
            mail.Subject = subject;
            mail.Body = message;
            mail.To.Add(ConfigurationManager.AppSettings["To"]);
            mail.IsBodyHtml = true;
            string port = ConfigurationManager.AppSettings["MailPort"];
            smtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["From"], ConfigurationManager.AppSettings["Password"]);
            smtpServer.EnableSsl = true;
            smtpServer.Port = Convert.ToInt32(port);
            smtpServer.Send(mail);
        }

        public static string PrepareEmailTemplate(bool isFailureTemplate, string fileName, string message, string errorMessage)
        {
            StringBuilder template = new StringBuilder();
            if (isFailureTemplate)
            {
                template.Append(ImportProvidersFailureTemplate);
                template.Replace("$Error$", errorMessage);
            }
            else
            {
                template.Append(ImportProvidersSuccessTemplate);
                template.Replace("$Message$", message);
            }

            template.Replace("$FileName$", fileName);
            template.Replace("$TimeStamp$", $"{DateTime.Now:MM/dd/yyyy HH:mm:ss}");

            return template.ToString();
        }

        private static string ImportProvidersFailureTemplate = @"
                                                                 <html>
                                                                    <head>
                                                                        <title>
                                                                            Provider Portal - Import Report
                                                                        </title>
                                                                    </head>
                                                                    <body>
                                                                        <h2>File Information</h2>
                                                                        <h4>Name: $FileName$</h4>
                                                                        <h4>TimeStamp: $TimeStamp$</h4>

                                                                        <h4 style='background-color:Yellow;'>Message: The file has some invalid records. Please correct and resubmit the file..</h4>

                                                                        <h2>Error:</h2>
                                                                        <h4>$Error$</h4>
                                                                    </body>
                                                                    </html>
                                                                ";
        private static string ImportProvidersSuccessTemplate = @"
                                                                 <html>
                                                                    <head>
                                                                        <title>
                                                                            Provider Portal - Import Report
                                                                        </title>
                                                                    </head>
                                                                    <body>
                                                                        <h2>File Information</h2>
                                                                        <h4>Name: $FileName$</h4>
                                                                        <h4>TimeStamp: $TimeStamp$</h4>

                                                                        <h4>Message: $Message$</h4>
                                                                    </body>
                                                                 </html>
                                                                ";
    }


}
