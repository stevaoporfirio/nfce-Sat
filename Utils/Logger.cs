using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;


namespace Utils
{
    public class Logger
    {
        private static readonly Logger instance = new Logger();
        private System.IO.StreamWriter file;
        private string name = AppDomain.CurrentDomain.BaseDirectory;

        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
            
        private Object _Lock = new Object();
        private Object _LockFile = new Object();

        private Queue<string> queue = new Queue<string>();

        private Logger() { }

        
        public static Logger getInstance
        {
            get
            {
                return instance;
            }
        }

        public void SetFileName(string _name)
        {
            name = AppDomain.CurrentDomain.BaseDirectory + _name;            
        }

        private void processLog(Object state)
        {
            _readWriteLock.EnterWriteLock();

            while (queue.Count > 0)
            {
                string msg = String.Empty;
                lock (_Lock)
                {
                        msg = queue.Dequeue();
                }
                lock (_LockFile)
                {
                    DateTime data = DateTime.Now;
                    file = new System.IO.StreamWriter(name + "_" + DateTime.Now.ToString("dd-MM-yy") +  ".log", true);
                    //if (file.BaseStream.Length > 1000000)
                    //{
                    //    System.IO.File.Copy(name + ".log", name + "-" + data.ToString("ddMMyyyy HHmmss") + ".log");

                    //    file.Flush();
                    //    file.Close(); //fechando arquivo para deletar

                    //    System.IO.File.Delete(name + ".log");
                    //    file = new System.IO.StreamWriter(name + ".log", true);
                    //}
                    file.WriteLine(data.ToString(System.Globalization.CultureInfo.InvariantCulture) + " :" + msg);
                    file.Flush();
                    file.Close();
                }
            }

            _readWriteLock.ExitWriteLock();

        }
        private void startConsumer()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(processLog));
        }
        public void error(string msg)
        {
            _readWriteLock.EnterWriteLock();
            lock (_Lock)
            {
                queue.Enqueue(msg);
            }
            startConsumer();
            _readWriteLock.ExitWriteLock();
           
        }
        public void error(Exception msg)
        {
            _readWriteLock.EnterWriteLock();
            lock (_Lock)
            {
                queue.Enqueue(msg.StackTrace);
            }
            startConsumer();
            _readWriteLock.ExitWriteLock();
        }
    }
}
