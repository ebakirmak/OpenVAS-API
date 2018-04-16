using OpenVAS;
using openVAS_API.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openVAS_API.PresentationLayer
{
    public class PLPort
    {
        public static string SetTargetPorts(OpenVASManager manager)
        {
            Console.WriteLine("Hedef Port adreslerini giriniz. -- 1-1000, 1005-1100 -- veya -- 1-65535 -- gibi.");           
            return BLPort.CreatePort(manager, "T: " + Convert.ToString(Console.ReadLine()));
        }
       

        public static string GetPortGUID(OpenVASManager manager)
        {
            BLPort.ListPorts(manager);
            return BLPort.GetPortGuid(manager, Convert.ToInt32(BLPort.SelectPort(manager)));

        }


    }
}
