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
        /*
         * Yeni bir target/hedef oluşturulur ve bu target/hedefin ID değeri döndürülür.
         * 
         */ 
        public static string CreateTarget(OpenVASManager manager, Guid portListID, string targetIPs,string excludeHost)
        {
            
            XDocument targetIPsXML = manager.CreateSimpleTarget(targetIPs, "C# Targets -- " + Guid.NewGuid().ToString(), portListID,excludeHost);
            string targetID = targetIPsXML.Root.Attribute("id").Value;
            return targetID;
        }
    }
}
