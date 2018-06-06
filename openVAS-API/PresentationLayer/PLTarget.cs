using OpenVAS;
using openVAS_API.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace openVAS_API.PresentationLayer
{
    /*
     * Bu sınıf, Target ayarları için sunum işlemlerini gören sınıftır.
     * 
     */
    class PLTarget
    {
        
        /*
         * Bu Fonksiyon Hedef/Target GUID değerini döndürür.
         * 
         */
        public static string GetTargetGUID(OpenVASManager manager)
        {
            //Set Port UUID
            string portChange = "";
            string targetPortGuid = "";
            do
            {
                Console.WriteLine("Port List oluşturmak istiyor musunuz? (E/H)");
                portChange = Console.ReadLine().ToString();

                //Port Create or Select
                if (portChange.ToUpper() == "E")
                    targetPortGuid = PLPort.CreatePortList(manager);
                else if (portChange.ToUpper() == "H")
                    targetPortGuid = PLPort.GetPortGUID(manager);

                if (targetPortGuid == null)
                {                 
                    return "0";
                }
                  

            } while (portChange.ToUpper() != "E" && portChange.ToUpper() != "H" && targetPortGuid == null );

           
            try
            {   
                //Set Target
                Console.WriteLine("Hedef IP adres(leri) giriniz. IP adreslerini virgül ile ayırınız.");
                string hostTargets = Console.ReadLine();
                //IPAddress iPAddress = IPAddress.Parse(hostTargets);

                Console.WriteLine("Hariç tutulacak IP adres(leri) giriniz. IP adreslerini virgül ile ayırınız.");
                string excludeTargets = Console.ReadLine();
                //IPAddress excludeIpAddress = IPAddress.Parse(excludeTargets);



                return BLTarget.CreateTarget(manager, new Guid(targetPortGuid), hostTargets,excludeTargets);
            }
            catch (Exception ex)
                {
                Console.WriteLine("IP Adress hatalı girdiniz. Lütfen sadece rakamla giriniz. Kontrol ediniz.\n" + ex.Message);
                return "0";
            }
           
        }


 

  


    }
}
