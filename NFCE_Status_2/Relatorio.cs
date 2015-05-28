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
    public partial class Relatorio : Form
    {
        int NFCEexecutadas = 0;
        int NFCEcanceladas = 0;
        int NFCEinutilizadas = 0;
        int NFCErejeitadas = 0;
        public Relatorio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataTable dt = ManagerDB.Instance.SelectNotasStatus(1, monthCalendar1.datasele;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            NFCEexecutadas = 0;
            NFCEcanceladas = 0;
            NFCEinutilizadas = 0;
            NFCErejeitadas = 0;
            DataTable dt = ManagerDB.Instance.RelatorioNFCE(dateTimePicker1.Value, dateTimePicker2.Value);
            foreach (DataRow row in dt.Rows)
            {
                string status = row["status"].ToString();
                switch (status)
                {
                    case "12":
                        NFCEexecutadas++;
                        break;
                    case "16":
                        NFCEcanceladas++;
                        break;
                    case "19":
                        NFCEinutilizadas++;
                        break;
                    default :
                        NFCErejeitadas++;
                        break;

                }

            }
            textBox1.Text = Convert.ToString(NFCEexecutadas);
            textBox2.Text = Convert.ToString(NFCEcanceladas);
            textBox3.Text = Convert.ToString(NFCEinutilizadas);
            textBox4.Text = Convert.ToString(NFCErejeitadas);

            try
            {
                folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

                DialogResult result = folderBrowserDialog1.ShowDialog();
                DateTime a = DateTime.Now;
                if (result == DialogResult.OK)
                    dt.WriteXml(folderBrowserDialog1.SelectedPath+"\\RelatorioNFCE.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
