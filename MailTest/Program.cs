using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Security;

namespace MailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            Start:
            program.Login();
            Console.WriteLine("For å skriv ny mail trykk enter.");
            Console.ReadLine();
            goto Start;
        }
        public void Login()
        {
            string mail = "";
            string message = "";
            string mottaker = "";
            try
            {
                MailAddress:
                Console.Write("Skriv inn mail adresse her:  ");
                mail = Console.ReadLine();
                if (mail=="")
                {
                    Console.WriteLine("Kan ikke stå tom!");
                    goto MailAddress;
                }
                else if(((mail.Contains("@hotmail")) || (mail.Contains("@gmail"))) != true)
                {
                    Console.WriteLine("Må inneholde et gyldig domene.");
                    goto MailAddress;
                }
                Password:
                SecureString password = getPasswordFromConsole("Skriv inn passordet her:  ");
                if (password.Length - 1 <= 0)
                {
                    Console.WriteLine("Passord kan ikke være tom!");
                    goto Password;
                }
                Console.WriteLine();
            mottaker:
                Console.Write("Skriv inn mottaker:  ");
                mottaker = Console.ReadLine();
                if (mottaker == "")
                {
                    Console.WriteLine("Du må skrive inn en mottaker.");
                    goto mottaker;
                }
            message:
                Console.Write("Skriv inn det du vil sende her:   ");
                message = Console.ReadLine();
                if (message == "")
                {
                    Console.WriteLine("Du kan ikke sende en tom mail.");
                    goto message;
                }
                
                Sendmail(mail, password, mottaker, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        public static SecureString getPasswordFromConsole(string dispalyMessage)
        {
            SecureString pass = new SecureString();
            Console.Write(dispalyMessage);
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                //Backspace should NOT work.
                if (!char.IsControl(key.KeyChar))
                {
                    pass.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass.RemoveAt(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }

            } while (key.Key != ConsoleKey.Enter);
            return pass;
        }
        public void Sendmail(string mailFrom, SecureString password,string mailTo,string mailmessage)
        {
            try
            {
                string clientHostMail = "";
                SmtpClient client = new SmtpClient();
                
                client.Port = 587;
                if (mailFrom.Contains("@hotmail"))
                {
                    clientHostMail = "smtp-mail.outlook.com";
                }
                else if(mailFrom.Contains("@gmail"))
                {
                    clientHostMail = "smtp.gmail.com";
                }
                client.Host = clientHostMail;
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mailFrom, password);

                MailMessage message = new MailMessage(mailFrom, mailTo, "Test", mailmessage);
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(message);
                Console.WriteLine("Mail sendt!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
