using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace SatServerAppContingenciaService
{
    public partial class ServiceContingencia : ServiceBase
    {
        invoiceServerApp.managers dtm;

        public ServiceContingencia()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";

            Thread.Sleep(15000);

            this.EventLog.WriteEntry("NFCE SAT Server Contingencia Inicializado", EventLogEntryType.Information);

            dtm = new invoiceServerApp.managers();
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(dtm.Run));
        }

        protected override void OnStop()
        {
        }
    }
}
