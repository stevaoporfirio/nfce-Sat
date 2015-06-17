using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    public class LoggerSAT
    {
        private static readonly LoggerSAT instance = new LoggerSAT();
        private System.IO.StreamWriter file;
        private string name = "LogSAT";
            
        private Object _Lock = new Object();
        private Object _LockFile = new Object();
        private Queue<string> queue = new Queue<string>();
        private LoggerSAT() { }


        public static LoggerSAT getInstance
        {
            get
            {
                return instance;
            }
        }
        private void processLog()
        {

            while (queue.Count != 0)
            {
                string msg = String.Empty;
                lock (_Lock)
                {
                    if(queue.Count > 0)
                        msg = queue.Dequeue();
                }
                lock (_LockFile)
                {
                    DateTime data = DateTime.Now;
                    file = new System.IO.StreamWriter(name + ".log", true);
                    if (file.BaseStream.Length > 1000000)
                    {
                        System.IO.File.Copy(name + ".log", name + "-" + data.ToString("ddMMyyyy HHmm") + ".log");

                        file.Flush();
                        file.Close(); //fechando arquivo para deletar

                        System.IO.File.Delete(name + ".log");
                        file = new System.IO.StreamWriter(name + ".log", true);
                    }
                    file.WriteLine(data.ToString(System.Globalization.CultureInfo.InvariantCulture) + " :" + msg);
                    file.Flush();
                    file.Close();
                }
            }


          
        }
        private void startConsumer()
        {
            Thread t = new Thread(new ThreadStart(processLog));
            t.Start();
        }
        public void error(string msg)
        {
            lock (_Lock)
            {
                queue.Enqueue(msg);
            }
            startConsumer();
           
        }
        public void error(Exception msg)
        {
            lock (_Lock)
            {
                queue.Enqueue(msg.StackTrace);
            }
            startConsumer();
        }

    }
}
