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
                    targetPortGuid = PLPort.SetTargetPorts(manager);
                else if (portChange.ToUpper() == "H")
                    targetPortGuid = PLPort.GetPortGUID(manager);
            } while (portChange.ToUpper() != "E" && portChange.ToUpper() != "H");

            //Set Target
            return BLTarget.CreateTarget(manager, new Guid(targetPortGuid));
        }
    }
}
