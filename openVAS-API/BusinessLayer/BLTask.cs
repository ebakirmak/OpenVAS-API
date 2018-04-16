using OpenVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openVAS_API.BL
{
    /*
     * Bu sınıf Task işlemlerinin gerçekleştirildiği İş Katmanı sınıfıdır.
     * 
     */
    public static class BLTask
    {
        /*
         * Yeni bir task oluşturulur.
         * 
         */
        public static void CreateTask(OpenVASManager manager, Guid policyID, Guid targetID)
        {         
      
            //Set Task
            XDocument task = manager.CreateSimpleTask("C# Tasks - " + Guid.NewGuid().ToString(), string.Empty, policyID, targetID);

            Guid taskID = new Guid(task.Root.Attribute("id").Value);

            manager.StartTask(taskID);

            XDocument status = manager.GetTasks(taskID);

            while (status.Descendants("status").First().Value != "Done")
            {
                Thread.Sleep(10000);
                Console.Write(status.Descendants(XName.Get("progress")).First().Nodes().OfType<XText>().First().Value + " - ");
                status = manager.GetTasks(taskID);
            }

            GetTaskReports(manager, taskID);


        }

        /*
         * Tasklar listelenir.
         * 
         */
        public static void ListTasks(OpenVASManager manager)
        {
            int counter = 0;
            XDocument tasks = manager.GetTasks();
            foreach (XElement task in tasks.Descendants(XName.Get("name")))
            {
                if (task.Value != "" && task.Parent.Name == "task")
                {
                    Console.WriteLine((counter + 1) + ") " + task.Value);
                    counter += 1;
                }
            }

        }

       

        /*
         * Seçilen taskın  GUID (Globally Unique Identifier) değerini döndürür.
         * 
         */
        public static string GetTaskGuid(OpenVASManager manager, int key)
        {
            int counter = 0;


            string taskGuid = "";

            XDocument tasks = manager.GetTasks();
            foreach (XElement task in tasks.Descendants(XName.Get("name")))
            {
                if (task.Value != "" && task.Parent.Name == "task")
                {
                    if (key - 1 == counter)
                    {
                        taskGuid = task.Parent.Attribute("id").Value;

                        return taskGuid;
                    }
                    counter += 1;

                }
            }
            return "0";
        }

        /*
         * Parametre olarak aldığı taskGUID'nin içerdiği ReportGUID'leri listeler.
         * 
         */
        private static List<string> ListReports(OpenVASManager manager, Guid taskGUID)
        {
            List<string> listReportGuid = new List<string>();

            string tmp = "";
            int counter = 1;
            XDocument tasks = manager.GetTasks(taskGUID);
            foreach (XElement task in tasks.Descendants(XName.Get("report")))
            {

                tmp = task.Attribute("id").Value;
                if (!listReportGuid.Contains(tmp))
                {
                    Console.WriteLine("Rapor " + counter + " :" + task.Attribute("id").Value);
                    listReportGuid.Add(tmp);
                    counter += 1;
                }

            }
            return listReportGuid;
          
        }

        /*
         * İstenilen raporun GUID'sini döndürür.
         * 
         */
        private static string GetReportGUID(List<string> listReportGuid)
        {
            Console.Write("Seçmek istediğiniz rapor numarası giriniz. (1,2 vb) :");
            int selectReport = Convert.ToInt32(Console.ReadLine());
            if (listReportGuid.Count >= selectReport)
                return listReportGuid.ElementAt(selectReport - 1);
            else
                return "0";
        }


        /*
         * Parametre olarak aldığı taskGUID'nin
         * 
         */
        public static bool GetTaskReports(OpenVASManager manager, Guid taskGUID)
        {
           
            string reportGuid = GetReportGUID(ListReports(manager, taskGUID));
            if(reportGuid != "0")
            {
                XDocument taskDetail = manager.GetTaskReports(new Guid(reportGuid));
                XElement firstChild = taskDetail.Root.Elements().First();
                //Console.WriteLine(firstChild.ToString());
                //taskDetail.Root.Parent.Remove();
                string strPath = Environment.GetFolderPath(
                             System.Environment.SpecialFolder.DesktopDirectory);
                //FirstChild.Save(strPath + "\\openvasReport.txt");
                System.IO.File.WriteAllText(strPath + "\\openvasReport.txt", firstChild.ToString());
                return true;
            }
            else
            {
                return false;
            }
               
        }
    }
}
