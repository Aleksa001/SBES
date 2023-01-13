using Common.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Common.Logger
{
    public enum AuditEventTypes
    {
        ReplicationInitiation = 0,
        ReplicationFailure = 1,
        ReplicationSuccess = 2
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string ReplicationInitiation
        {
            get
            {
            
                return ResourceMgr.GetString(AuditEventTypes.ReplicationInitiation.ToString());
            }
        }

        public static string ReplicationFailure
        {
            get
            {
               
                return ResourceMgr.GetString(AuditEventTypes.ReplicationFailure.ToString());
            }
        }

        public static string ReplicationSuccess
        {
            get
            {
            
                return ResourceMgr.GetString(AuditEventTypes.ReplicationSuccess.ToString());
            }
        }
    }
}
