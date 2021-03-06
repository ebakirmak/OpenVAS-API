﻿using OpenVAS;
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
            PLSession.SetIPAndPort();

            using (OpenVASSession session = new OpenVASSession(PLSession.Username, PLSession.Password, PLSession.IP, PLSession.Port))
            {
                using (OpenVASManager manager = new OpenVASManager(session))
                {
                    if (session.Stream != null && session.Username != null && session.Password != null)
                    {
                        string change;
                        bool isReport = true;
                        do
                        {
                            Console.Write("" +
                                            "\nAşağıdaki işlemlerden birini seçiniz. \n" +
                                            "* Raporları getirmek için 'R' basınız. \n" +
                                            "* Yeni bir tarama için 'T' basınız.\n" +
                                            "* OpenVAS Management Protocol Versiyonu İçin 'V' basınız.\n" +
                                            "* Test için 'E' basınız.\n" +
                                            "* Çıkış için 'Q' basınız.\n");

                            Console.Write("Seçim: ");
                            change = Console.ReadLine().ToUpper();

                            if (change.ToUpper() == "T")
                            {
                                PLTask.CreateTask(manager);
                            }
                            else if (change.ToUpper() == "R")
                            {
                                PLTask.ListTasks(manager);
                                Console.Write("\nRapor Seçiniz.\n");
                                string ReportGUID = BLTask.GetTaskGuid(manager, Convert.ToInt32(PLTask.SelectTask(manager)));
                                if (ReportGUID == "0")
                                    Console.WriteLine("İlgili Task bulunamadı...");
                                else
                                    isReport = BLTask.GetTaskReports(manager, new Guid(ReportGUID));

                                if (!isReport)
                                    Console.Write("İlgili Rapor Bulunamadı. Girilen değeri kontrol ediniz.\n");
                            }
                            else if (change.ToUpper() == "V")
                                Console.WriteLine(manager.GetVersion());
                            else if (change.ToUpper() == "E")
                                session.TestStream();
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
