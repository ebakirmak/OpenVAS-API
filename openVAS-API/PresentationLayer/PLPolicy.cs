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
     * Bu sınıf, Policy ayarları için sunum işlemlerini gören sınıftır.
     * 
     */
    public class PLPolicy
    {
        public static string GetPolicyGUID(OpenVASManager manager)
        {
            BLPolicy.ListPolicys(manager);

            int key = SelectPolicy(manager);

            //Set Policy UUID              
            return BLPolicy.GetPolicyGUID(manager,key);
        }

        //Policy was selected.
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
