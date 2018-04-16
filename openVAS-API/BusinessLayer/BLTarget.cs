using OpenVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openVAS_API.BL
{
    public static class BLTarget
    {
        // Create Target
        public static string CreateTarget(OpenVASManager manager, Guid portListID)
        {

            Console.WriteLine("Hedef IP adres(leri) giriniz. IP adreslerini virgül ile ayırınız.");
            string targetIPs = Convert.ToString(Console.ReadLine());
            XDocument targetIPsXML = manager.CreateSimpleTarget(targetIPs, "C# Targets -- " + Guid.NewGuid().ToString(), portListID);
            string targetID = targetIPsXML.Root.Attribute("id").Value;
            return targetID;
        }
    }
}
