using OpenVAS;
using openVAS_API.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openVAS_API.PresentationLayer
{
    public class PLTask
    {


        public static void ListTasks(OpenVASManager manager)
        {
            Console.Write("\nTask Seçiniz.\n");
            BLTask.ListTasks(manager);
     
        }

        /*
        * Task seçilir.
        * 
        */
        public static string SelectTask(OpenVASManager manager)
        {
            bool tmp = false;
            string taskContent = "";
            do
            {
                Console.Write("İlgili Task için ID girmeniz yeterlidir: ");
                taskContent = Console.ReadLine();
                int taskID = 0;
                if (int.TryParse(taskContent, out taskID))
                {
                    tmp = true;
                    return Convert.ToString(taskID);
                }
                else
                {
                    Console.WriteLine("Lütfen değeri kontrol ediniz.");
                }
            } while (tmp == false);



            return "1";

        }

    }
}
