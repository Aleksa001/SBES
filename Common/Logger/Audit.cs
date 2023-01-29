using Common.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common.Logger
{
    public class Audit : IDisposable
    {

        private static EventLog customLog = null;
        const string SourceName = "SecurityManager.Audit";
        const string LogName = "MySecTest";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void ReplicationInitiated()
        {
            

            if (customLog != null)
            {
                string replInit =
                    AuditEvents.ReplicationInitiation;
                string message = String.Format(replInit);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ReplicationInitiation));
            }
        }

        public static void ReplicationSuccess()
        {
           
            if (customLog != null)
            {
                string replSuccess =
                    AuditEvents.ReplicationSuccess;
                string message = String.Format(replSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ReplicationSuccess));
            }
        }


        public static void ReplicationFailed()
        {
            if (customLog != null)
            {
                string replFailed =
                    AuditEvents.ReplicationFailure;
                string message = String.Format(replFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ReplicationFailure));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
