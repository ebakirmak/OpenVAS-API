using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVAS
{
    /* 
     * Bu sınıf OpenVAS Management Protocol komutlarını içeren sınıftır.
     * Bu komutlar; task oluşturmak, port listesi oluşturmak, policy'leri listelemek ve OpenVAS arayüzünden yapabildiğiniz herşey olabilir.
     */

    /*
     * This class is class including OpenVAS Management Protocol (OMP) commands.
     * This commands, to create task, to create port list, to list policies and could be everything that you can do from OpenVAS GUI.     * 
     */

    public class OpenVASManager : IDisposable
    {
        /*
         * Bu değişken, OpenVAS'da oturum açmak için kullanılır.
         * This variable is used for login in OpenVAS.
         */
        private OpenVASSession _session;

        public OpenVASManager(OpenVASSession session)
        {
            if (session != null)
                _session = session;
        }

        /*
         * OpenVAS Management Protocol versiyonunu getirir.
         * Get the OpenVAS Manager Protocol version.
         */
        public XDocument GetVersion()
        {
            return _session.ExecuteCommand(XDocument.Parse("<get_version />"));
        }

        /*
         * istemci Config/Policy bilgilerini getirmek için get_configs komutunu kullanır.
         * The client uses the get_configs command to get config information.
         */
        public XDocument GetScanConfigurations()
        {
            return _session.ExecuteCommand(XDocument.Parse("<get_configs />"), true);
        }

        /*
         * İstemci yeni bir port listesi oluşturmak için create_port_list komutunu kullanır.
         * The client uses the create_port_list command to create a new port list.
         */
        public XDocument CreateSimplePort(string targetName, string comment, string portRange)
        {
            XDocument createPortXML = new XDocument(
                                        new XElement("create_port_list",
                                            new XElement("name", targetName),
                                            new XElement("comment", comment),
                                            new XElement("port_range", portRange)));

            return _session.ExecuteCommand(createPortXML, true);
        }

        /*
         * İstemci port list bilgisini getirmek için get_port_lists komutunu kullanır.
         * The client uses the get_port_lists command to get port list information.
         */
        public XDocument GetPortLists()
        {
            XDocument createPortXML = new XDocument(
                                       new XElement("get_port_lists"));

            return _session.ExecuteCommand(createPortXML, true);
        }

        /*
         * İstemci yeni bir hedef/target oluşturmak için create_target komutunu kullanır.
         * The client uses the create_target command to create a new target.
         */
        public XDocument CreateSimpleTarget(string cidrRange, string targetName, Guid portListID)
        {

            XDocument createTargetXML = new XDocument(
                                            new XElement("create_target",
                                                new XElement("name", targetName),
                                                new XElement("hosts", cidrRange),
                                                new XElement("port_list",
                                                    new XAttribute("id", portListID.ToString()))));

            return _session.ExecuteCommand(createTargetXML, true);
        }

        /*
         * İstemci yeni bir task oluşturmak için create_task komutunu kullanır.
         * The client uses the create_task command to create a new task.
         */
        public XDocument CreateSimpleTask(string name, string comment, Guid configID, Guid targetID)
        {

            XDocument createTaskXML = new XDocument(
                                          new XElement("create_task",
                                              new XElement("name", name),
                                              new XElement("comment", comment),
                                              new XElement("config",
                                                  new XAttribute("id", configID.ToString())),
                                              new XElement("target",
                                                  new XAttribute("id", targetID.ToString()))));


            return _session.ExecuteCommand(createTaskXML, true);
        }

        /*
         * İstemci mevcut bir taskı manuel olarak başlatmak için start_task komutunu kullanır.
         * The client uses the start_task command to manually start an existing task.
         */
        public XDocument StartTask(Guid taskID)
        {
            XDocument startTaskXML = new XDocument(
                                         new XElement("start_task",
                                             new XAttribute("task_id", taskID.ToString())));

            return _session.ExecuteCommand(startTaskXML, true);
        }

        /*
         * İstemci task bilgilerini getirmek için get_tasks komutunu kullanır.
         * The client uses the get_tasks command to get task information.
         */
        public XDocument GetTasks(Guid? taskID = null)
        {

            if (taskID != null)
                return _session.ExecuteCommand(new XDocument(
                    new XElement("get_tasks",
                        new XAttribute("task_id", taskID.ToString()))), true);

            return _session.ExecuteCommand(XDocument.Parse("<get_tasks />"), true);
        }

        /*
         * İstemci rapor bilgilerini getirmek için get_reports komutunu kullanır.
         * The client uses the get_reports command to get report information.
         */
        public XDocument GetTaskReports(Guid reportID)
        {
            XDocument getTaskReportXML = new XDocument(
                                              new XElement("get_reports",                                          
                                                  new XAttribute("report_id", reportID)));

            return _session.ExecuteCommand(getTaskReportXML, true);
        }


        public void Dispose()
        {
            _session.Dispose();
        }

    }
}
