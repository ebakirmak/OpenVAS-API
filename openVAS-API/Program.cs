using OpenVAS;
using openVAS_API.BL;
using openVAS_API.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openVAS_API
{
    class Program
    {
        /* Here is Protocol Documentaion
        * http://docs.greenbone.net/API/OMP/omp-7.0.html
        * */

        /*OpenVAS need to username, password, 
         * ip that service is running and 
         * port that openvasmd service is running...
         */

        /*
         * You have to pay attention your firewall 
         * because it is blocking ports except known ports..
         */

        public static void Main(string[] args)
        {
            string remote_server_ip = "";        
            using (OpenVASSession session = new OpenVASSession("admin", "admin", remote_server_ip, 9390))
            {
                using (OpenVASManager manager = new OpenVASManager(session))
                {



                    if (session.Stream != null && session.Username != null && session.Password != null)
                    {
                        string change;
                        do
                        {
                            Console.Write("" +
                                            "\nAşağıdaki işlemlerden birini seçiniz. \n" +
                                            "* Raporları getirmek için 'R' basınız. \n" +
                                            "* Yeni bir tarama için 'T' basınız.\n" +
                                            "* OpenVAS Management Protocol Versiyonu İçin 'V' basınız.\n"+
                                            "* Çıkış için 'Q' basınız.\n");

                            Console.Write("Seçim: ");
                            change = Console.ReadLine().ToUpper();

                            if (change.ToUpper() == "T")
                            {
                                BLTask.CreateTask(session, manager,new Guid(PLPolicy.GetPolicyGUID(manager)),new Guid(PLTarget.GetTargetGUID(manager)));
                            }
                            else if (change.ToUpper() == "R")
                            {
                                PLTask.ListTasks(manager);                               
                                Console.Write("\nRapor Seçiniz.\n");
                                BLTask.GetTaskReports(manager, new Guid(BLTask.GetTaskGuid(manager, Convert.ToInt32(PLTask.SelectTask(manager)))));
                            }
                            else if (change.ToUpper() == "V")
                                Console.WriteLine(manager.GetVersion());
                            else if (change.ToUpper() != "Q")
                                Console.WriteLine("Geçersiz işlem. Tekrar Deneyiniz.");
                     

                        } while (change.ToUpper() != "Q");


                    }
                }
                Console.Write("İşlem sonlandırıldı.");
                Console.ReadLine();
            }


        }


       

      
    }
}
