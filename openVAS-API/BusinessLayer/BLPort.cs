using OpenVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openVAS_API.BL
{
   public static class BLPort
    {
        /*
         * Parametre olarak alınan targetPorts aralığında Port List oluşturulur. Örneğin 80 ile 8088 arasındaki portlar taransması için '80-8088'  girilir.
         * Oluşturulan Port List'in ID döndürülür ve bu ID yeni Task'ın Target Port'u olarak kullanılır.
         * 
         */
        public static string CreatePort(OpenVASManager manager, string targetPorts)
        {
            try
            {
                XDocument targetPortsXML = manager.CreateSimplePort("Port: " + targetPorts.Split('T')[1] +"  -- " + Guid.NewGuid().ToString(), "Deneme", targetPorts);
                string targetPortsID = targetPortsXML.Root.Attribute("id").Value;
                return targetPortsID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        /*
         * Kayıtlı olan ve bizim oluşturduğumuz Port Listeleri listelenir.All TCP, All TCP and Nmap 5.51 top 100 UDP ve C# Ports -- xxxx.xxxx.xxxx.xxxx
         * 
         */
        public static void ListPorts(OpenVASManager manager)
        {
            int i = 0;
            XDocument configs = manager.GetPortLists();
            foreach (XElement node in configs.Descendants(XName.Get("name")))
            {
                if (node.Value != "" && node.Value != "admin" && node.Value != "Everything")
                {
                    Console.WriteLine(i + 1 + ") " + node.Value);
                    i += 1;
                }
            }
        }



        /*
         * Seçilen Port List'in ID döndürülür. (Key, Terminal ekranında 'İlgili Port için ID girmeniz yeterlidir:' kısmında girilen değerdir.)
         * 
         */
        public static string GetPortID(OpenVASManager manager,int key)
        {
           




            string portListGuid = "";
            int counter = 0;
            XDocument configs = manager.GetPortLists();
            foreach (XElement node in configs.Descendants(XName.Get("name")))
            {
                if (node.Value != "" && node.Value != "admin" && node.Value != "Everything")
                {
                    if (counter == key-1)
                    {
                        portListGuid = node.Parent.Attribute("id").Value;
                        return portListGuid;
                    }
                    else
                        counter += 1;
                }

            }
            return null;
        }
    }
}
