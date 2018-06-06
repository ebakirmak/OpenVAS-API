using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openVAS_API.PresentationLayer
{
    public class PLSession
    {


        //Server IP
        public static string IP { get; set; }
        //Server Port
        public static int Port { get; set; }
        //Username
        public static string Username { get; set; }
        //Password
        public static string Password { get; set; }


        /// <summary>
        /// Bu fonksiyon ip ve port numaralarını ayarlar.
        /// This function sets up ip and port number.
        /// </summary>
        public static void SetIPAndPort()
        {
            do
            {
                try
                {

                    Console.Write("IP Adresi ve Port Adresini değiştirmek istiyor musunuz?(E/H)");
                    string selected = Console.ReadLine().ToUpper();
                    if (selected == "E")
                    {
                        Console.Write("IP Adresini Giriniz: ");
                        IP = Console.ReadLine();

                        Console.Write("Port Numarasını Giriniz: ");
                        Port = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Username Giriniz: ");
                        Username = Console.ReadLine();

                        Console.Write("Parola Giriniz: ");
                        Password = Console.ReadLine();

                        break;
                    }
                    else if (selected == "H")
                    {
                        IP = "172.17.6.4";
                        Port = 9390;
                        Username = "admin";
                        Password = "password";
                
                        break;
                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine("Input format biçimi hatalı. Kontrol ediniz." + e.Message);
                }
            } while (true);


        }
    }
}
