using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.Threading;
using invoiceServerApp;

namespace SatServerAppService
{
    public partial class ServiceNFCeSAT : ServiceBase
    {
        Datamanager dtm;

        public ServiceNFCeSAT()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";
            
            this.EventLog.WriteEntry("NFCE SAT Server Inicializado", EventLogEntryType.Information);            

            dtm = new Datamanager();
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(dtm.Run));
        }

        protected override void OnStop()
        {
        }
    }
}
