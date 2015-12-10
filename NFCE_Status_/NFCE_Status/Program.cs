using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NFCE_Status
{
    static class Program
    {
        //public static MicrosDB microsDB = new MicrosDB("micros", "custom", "custom");
        public static AppControle appControle;


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
