using OpenVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openVAS_API.BL
{
    public static class BLPolicy
    {
        /*
         * Policy/Config Listelenir. Policy tarama ayarıdır. Full and Fast gibi seçenekleri mevcuttur.
         * 
         */
         
        public static void ListPolicys(OpenVASManager manager)
        {
            int i = 0;
            XDocument configs = manager.GetScanConfigurations();
            foreach (XElement node in configs.Descendants(XName.Get("name")))
            {
                int j= 0;
                foreach (XElement comment in configs.Descendants(XName.Get("comment")))
                {

                    if (node.Value == "")
                        break;

                        if (i == j)
                        {
                            Console.WriteLine(i + 1 + ") " + node.Value + ": " + comment.Value);
                            i += 1;
                            break;
                        }
                        
                        j += 1;                  
                   
                }
               
            }









        }



       /*
        * Parametrede yer alan key sırasındaki ilgili Policy'nin ID değerini döndürür. 
        * 
        */
        public static string GetPolicyGUID(OpenVASManager manager, int key)
        {     

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
            return "0";
        }

    }
}
