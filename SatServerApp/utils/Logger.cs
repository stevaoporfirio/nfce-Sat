using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace invoiceServerApp
{
    public class Logger
    {
        private static readonly Logger instance = new Logger();
        private System.IO.StreamWriter file;
        private Logger() { }
        public static Logger getInstance
        {
            get
            {
                return instance;
            }
        }
        public void error(string msg)
        {
            DateTime dat1 = DateTime.Now;
            file = new System.IO.StreamWriter("SatServerApp.log",true);
            file.WriteLine(dat1.ToString(System.Globalization.CultureInfo.InvariantCulture)+ " :" + msg);
            file.Flush();
            file.Close();
        }
        public void error(Exception msg)
        {
            DateTime dat1 = DateTime.Now;
            file = new System.IO.StreamWriter("SatServerApp.log", true);
            file.WriteLine(dat1.ToString(System.Globalization.CultureInfo.InvariantCulture) + " :" + msg.StackTrace);
            file.Flush();
            file.Close();
        }
    }
}
