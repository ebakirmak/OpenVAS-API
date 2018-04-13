using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVAS
{
    public class OpenVASManager : IDisposable
    {
        private OpenVASSession _session;

        public OpenVASManager(OpenVASSession session)
        {
            if (session != null)
                _session = session;
        }

        public XDocument GetVersion()
        {
            return _session.ExecuteCommand(XDocument.Parse("<get_version />"));
        }

        public XDocument GetScanConfigurations()
        {
            return _session.ExecuteCommand(XDocument.Parse("<get_configs />"), true);
        }

        public XDocument CreateSimplePort(string targetName, string comment, string portRange)
        {
            XDocument createPortXML = new XDocument(
                                        new XElement("create_port_list",
                                            new XElement("name", targetName),
                                            new XElement("comment", comment),
                                            new XElement("port_range", portRange)));

            return _session.ExecuteCommand(createPortXML, true);
        }

        public XDocument GetPortLists()
        {
            XDocument createPortXML = new XDocument(
                                       new XElement("get_port_lists"));

            return _session.ExecuteCommand(createPortXML, true);
        }

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

        public XDocument StartTask(Guid taskID)
        {
            XDocument startTaskXML = new XDocument(
                                         new XElement("start_task",
                                             new XAttribute("task_id", taskID.ToString())));

            return _session.ExecuteCommand(startTaskXML, true);
        }


        public XDocument GetTasks(Guid? taskID = null)
        {

            if (taskID != null)
                return _session.ExecuteCommand(new XDocument(
                    new XElement("get_tasks",
                        new XAttribute("task_id", taskID.ToString()))), true);

            return _session.ExecuteCommand(XDocument.Parse("<get_tasks />"), true);
        }


        public XDocument GetTaskReports(Guid reportID)
        {
            XDocument getTaskReportXML = new XDocument(
                                              new XElement("get_reports",
                                                  //new XAttribute("report_id", "6bf99b28-ccf6-46de-b039-00614e14b371"),
                                                  new XAttribute("report_id", reportID)));

            return _session.ExecuteCommand(getTaskReportXML, true);
        }


        public void Dispose()
        {
            _session.Dispose();
        }

    }
}
