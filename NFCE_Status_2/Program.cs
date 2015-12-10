using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace invoiceServerApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static int argDB;

        [STAThread]
        static void Main(string[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                try
                {
                    Int32.TryParse(args[0], out argDB);
                }
                catch (Exception ex)
                {
                    //
                    argDB = 0;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            
        }
    }
}
