using OpenVAS;
using openVAS_API.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openVAS_API.PresentationLayer
{
    /*
     * Bu sınıf, Port ayarları için sunum işlemlerini gerçekleştirir.
     * 
     */ 
    public class PLPort
    {
        /*
         * Yeni bir Hedef Port listesi tanımlanır ve ID değeri döndürülür.
         *
         */ 
        public static string CreatePortList(OpenVASManager manager)
        {
            Console.WriteLine("Hedef Port adreslerini giriniz. -- 1-1000, 1005-1100 -- veya -- 1-65535 -- gibi.");           
            return BLPort.CreatePort(manager, "T: " + Convert.ToString(Console.ReadLine()));
        }
       
        /*
         * Target Port Listesinden seçilen  target listin ID değerini döndürür.
         * 
         */ 
        public static string GetPortGUID(OpenVASManager manager)
        {
            BLPort.ListPorts(manager);
            return BLPort.GetPortID(manager, Convert.ToInt32(SelectPort(manager)));
        }
  
        /*
         * Port Listesi seçilir.
         * 
         */ 
        private static string SelectPort(OpenVASManager manager)
        {
            bool tmp = false;
            string portList = "";
            do
            {
                Console.Write("İlgili Port için ID girmeniz yeterlidir: ");
                portList = Console.ReadLine();
                int portListID = 0;
                if (int.TryParse(portList, out portListID))
                {
                    tmp = true;
                    return Convert.ToString(portListID);
                }
                else
                {
                    Console.WriteLine("Lütfen deðeri kontrol ediniz.");
                }
            } while (tmp == false);



            return "1";

        }

    }
}
