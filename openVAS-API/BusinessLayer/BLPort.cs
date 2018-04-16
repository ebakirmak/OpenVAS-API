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
        //Create Port List
        public static string CreatePort(OpenVASManager manager, string targetPorts)
        {
           
            XDocument targetPortsXML = manager.CreateSimplePort("C# Ports -- " + Guid.NewGuid().ToString(), "Deneme", targetPorts);
            string targetPortsID = targetPortsXML.Root.Attribute("id").Value;
            return targetPortsID;
        }

        //Port List was listed.
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

        //Port List was selected
        public static string SelectPort(OpenVASManager manager)
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

        //Port List was got
        public static string GetPortGuid(OpenVASManager manager,int key)
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
