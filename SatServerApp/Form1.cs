using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace invoiceServerApp
{
    public partial class Form1 : Form
    {
        private Datamanager manager;
        public Form1()
        {
            InitializeComponent();
        }
         ~Form1()
        { 
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string b = dllSat.ConsultarSAT(6);
            //MessageBox.Show(b);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;

            manager = new Datamanager();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //this.Visible = true;
            //this.WindowState = FormWindowState.Normal;
            //this.ShowInTaskbar = true;
            //notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //ContextMenu menu = System.
            //notifyIcon1.ContextMenu.MenuItems.Add("Reinstart", Application.Restart
            Application.Exit();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.Abort();
            Application.Restart();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult a = new System.Windows.Forms.DialogResult();
            a = MessageBox.Show(null,"Deseja realmente fechar o programa. ","Aviso ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (a == System.Windows.Forms.DialogResult.OK)
            {
                System.Threading.Thread.CurrentThread.Abort();
                Application.Exit();
            }
            
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
