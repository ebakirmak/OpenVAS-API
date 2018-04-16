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
     * Bu sınıf, Policy ayarları için sunum işlemlerini gerçekleştirir.
     * 
     */
    public class PLPolicy
    {
        /*
         * Policy'ler listelenir. Policy Seçilir. Seçilen Policy'nin id değeri döndürülür.
         * 
         */ 
        public static string GetPolicyGUID(OpenVASManager manager)
        {
            BLPolicy.ListPolicys(manager);

            int key = SelectPolicy(manager);
                        
            return BLPolicy.GetPolicyGUID(manager,key);
        }

        /*
         * Policy seçilir.
         * 
         */ 
        private  static int SelectPolicy(OpenVASManager manager)
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
                    return policyID;
                }
                else
                {
                    Console.WriteLine("Lütfen deðeri kontrol ediniz.");
                }
            } while (tmp == false);

            return 0;

        }

    }
}
