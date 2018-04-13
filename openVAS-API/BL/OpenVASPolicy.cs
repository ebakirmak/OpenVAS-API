using OpenVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openVAS_API.BL
{
    public static class OpenVASPolicy
    {
        //Policys were listed.
        public static void ListPolicys(OpenVASManager manager)
        {
            int i = 0;
            XDocument configs = manager.GetScanConfigurations();
            foreach (XElement node in configs.Descendants(XName.Get("name")))
            {
                if (node.Value != "")
                {
                    Console.WriteLine(i + 1 + ") " + node.Value);
                    i += 1;
                }
            }
        }

        //Policy was selected.
        public static string SelectPolicy(OpenVASManager manager)
        {


            bool tmp = false;
            do
            {
                Console.Write("İlgili Policy için ID girmeniz yeterlidir: ");
                string policy = Console.ReadLine();
                int policyID = 0;
                if (int.TryParse(policy, out policyID))
                {
                    tmp = true;
                    return Convert.ToString(policyID);
                }
                else
                {
                    Console.WriteLine("Lütfen deðeri kontrol ediniz.");
                }
            } while (tmp == false);



            return "4";


        }

        //Policy was got.
        public static string GetPolicy(OpenVASManager manager)
        {
            //List Policys
            ListPolicys(manager);

            //Select Policy ID
            int key = Convert.ToInt32(SelectPolicy(manager));


            string policy = "";
            int counter = 0;
            XDocument configs = manager.GetScanConfigurations();
            foreach (XElement node in configs.Descendants(XName.Get("name")))
            {
                if (node.Value != "")
                {
                    if (counter == key-1)
                    {
                        policy = node.Parent.Attribute("id").Value;
                        return policy;
                    }
                    else
                        counter += 1;
                }

            }
            return policy = "Full and Fast ultimate";
        }
    }
}
