using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net.Sockets;

namespace NFCE_Status
{
    public partial class Form1 : Form
    {
        private NFCE_Data nfd;
        private int num;

        public Form1(NFCE_Data nd, int _num)
        {
            nfd = nd;
            num = _num;

            InitializeComponent();

            Define();
        }

        private void Define()
        {
            txtNumNFCe.Text = nfd.Numero.ToString();
            txtNumConta.Text = nfd.Chk_ID.ToString();
            txtWSID.Text = nfd.WS_ID.ToString();
            txtChaveAcesso.Text = nfd.NFCE_Key;            
            txtNumRecibo.Text = nfd.NFCE_Recibo.ToString();
            txtDtEmissao.Text = nfd.NFCE_DateTime.ToString("dd-MM-yyyy hh:mm:ss");
            txtDtEmissaoCont.Text = nfd.NFCE_DateTimeCont.ToString("dd-MM-yyyy hh:mm:ss");

            richTextBox1.Text = nfd.Info;

            dataGridView1.DataSource = nfd.Status_List;
            
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;

            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[2].Width = 150;
            //dataGridView1.Columns[3].Width = 1000;

            dataGridView1.Columns[1].HeaderText = "Status";
            dataGridView1.Columns[2].HeaderText = "Data/Hora";
            //dataGridView1.Columns[3].HeaderText = "Info";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //btnOk.Focus();
        }
        private string getCFGConfig(string _key)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"config_nfce_sat.cfg");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split('=');

                if (words.Length == 2)
                {
                    foreach (string word in words)
                    {
                        if(word.Equals(_key))
                            return words[1];
                    }
                }
            }
            file.Close();
            return "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            string msg = "CANCEL|" + nfd.Id + "|END|";
            MessageBox.Show(sendNfce(msg));
            DialogResult = DialogResult.OK;
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string msg = "INUTILIZACAO|" +nfd.Id+ "|" + nfd.Numero + "|END|";
            MessageBox.Show(sendNfce(msg));
            DialogResult = DialogResult.OK;
        }
        private string sendNfce(string _msg)
        {
            string ret = String.Empty;
            try
            {
                TcpClient client = new TcpClient(getCFGConfig("IP_SERVER"), Convert.ToInt32(getCFGConfig("PORTA")));
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(_msg);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                data = new Byte[8192];
                Int32 bytes = stream.Read(data, 0, data.Length);
                ret = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
            }

            return ret;
        }



    }
}
