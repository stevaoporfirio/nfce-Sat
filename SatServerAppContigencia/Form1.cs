using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace invoiceServerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            managers a = new managers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;
            //Utils.Logger.getInstance.name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            managers a = new managers();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {

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

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult a = new System.Windows.Forms.DialogResult();
            a = MessageBox.Show(null, "Deseja realmente fechar o programa. ", "Aviso ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (a == System.Windows.Forms.DialogResult.OK)
                Application.Exit();
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult a = new System.Windows.Forms.DialogResult();
            a = MessageBox.Show(null, "Deseja realmente fechar o programa. ", "Aviso ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (a == System.Windows.Forms.DialogResult.OK)
                Application.Exit();
        }

        private void restartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }
    }
}
