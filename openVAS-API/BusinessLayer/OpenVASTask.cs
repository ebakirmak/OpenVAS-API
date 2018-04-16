﻿using OpenVAS;
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
    public static class OpenVASTask
    {
        /*
         * Yeni bir task oluşturulur.
         * 
         */
        public static void CreateTask(OpenVASSession session, OpenVASManager manager)
        {
            //Set Policy UUID  
            
            string policyGuid = OpenVASPolicy.GetPolicy(manager);
            string portChange = "";
            string targetPortGuid="";
            do
            {
                Console.WriteLine("Port List oluşturmak istiyor musunuz? (E/H)");
                portChange = Console.ReadLine().ToString();
               
                //Port Create or Select
                if (portChange.ToUpper() == "E")
                    targetPortGuid = OpenVASPort.CreatePort(manager);
                else if (portChange.ToUpper() == "H")
                    targetPortGuid = OpenVASPort.GetPortGuid(manager);
            } while (portChange.ToUpper() != "E" && portChange.ToUpper() != "H");
         



            //Set Target
            string targetGuid = OpenVASTarget.CreateTarget(manager, new Guid(targetPortGuid));


            //Set Port UUID

            //Set Task
            XDocument task = manager.CreateSimpleTask("C# Tasks - " + Guid.NewGuid().ToString(), string.Empty, new Guid(policyGuid), new Guid(targetGuid));

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
         * Task seçilir.
         * 
         */
        public static string SelectTask(OpenVASManager manager)
        {
            bool tmp = false;
            string taskContent = "";
            do
            {
                Console.Write("İlgili Task için ID girmeniz yeterlidir: ");
                taskContent = Console.ReadLine();
                int taskID = 0;
                if (int.TryParse(taskContent, out taskID))
                {
                    tmp = true;
                    return Convert.ToString(taskID);
                }
                else
                {
                    Console.WriteLine("Lütfen değeri kontrol ediniz.");
                }
            } while (tmp == false);



            return "1";

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
            return "";
        }

        /*
         * Parametre olarak aldığı taskGUID'nin içerdiği ReportGUID'leri döndürür.
         * 
         */
        private static string GetReportGuid(OpenVASManager manager, Guid taskGUID)
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

            Console.Write("Seçmek istediğiniz rapor numarası giriniz. (1,2 vb) :");
            int selectReport = Convert.ToInt32(Console.ReadLine());


            return listReportGuid.ElementAt(selectReport - 1);
        }

        /*
         * Parametre olarak aldığı taskGUID'nin
         * 
         */
        public static void GetTaskReports(OpenVASManager manager, Guid taskGUID)
        {

            string reportGuid = GetReportGuid(manager, taskGUID);
            XDocument taskDetail = manager.GetTaskReports(new Guid(reportGuid));
            XElement firstChild = taskDetail.Root.Elements().First();
            //Console.WriteLine(firstChild.ToString());
            //taskDetail.Root.Parent.Remove();
            string strPath = Environment.GetFolderPath(
                         System.Environment.SpecialFolder.DesktopDirectory);
            //FirstChild.Save(strPath + "\\openvasReport.txt");
            System.IO.File.WriteAllText(strPath + "\\openvasReport.txt", firstChild.ToString());
        }
    }
}