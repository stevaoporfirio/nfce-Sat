using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace invoiceServerApp
{
    public partial class Consultar : Form
    {
        private string chave = "";
        private Dictionary<string, string> listImpressora = new Dictionary<string, string>();
        public Consultar()
        {
            InitializeComponent();
            button2.Enabled = false;
            getWSID();
            comboBox1.DataSource = new BindingSource(listImpressora, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string chave = textBox2.Text;
            string conta = textBox1.Text;
            if(conta !="")
                ConsultaConta(Convert.ToInt32(conta));
            else if (chave != "")
                ConsultaChave(chave);

        }
        private void ConsultaConta(int _number)
        {
            DataTable dt_conta = ManagerDB.Instance().SelectNotaConta(_number);
            if (dt_conta.Rows.Count != 0)
            {
                foreach (DataRow row in dt_conta.Rows)
                {
                    string id = row["id"].ToString();
                    chave = row["chave"].ToString();
                    dataGridView1.DataSource = ManagerDB.Instance().SelectStatusNotas(id);
                    button2.Enabled = true;
                    return;
                }
            
            }
            else
                MessageBox.Show("Conta Nao Encontrada.");
        }
        private void ConsultaChave(string _chave)
        {
            DataTable dt_chave = ManagerDB.Instance().SelectNotaChave(_chave);
            if (dt_chave.Rows.Count != 0)
            {
                foreach (DataRow row in dt_chave.Rows)
                {
                    string id = row["id"].ToString();
                    chave = row["chave"].ToString();
                    dataGridView1.DataSource = ManagerDB.Instance().SelectStatusNotas(id);
                    button2.Enabled = true;
                    return;
                }

            }
            else
                MessageBox.Show("Conta Nao Encontrada.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string wsid = getCFGConfig("IP_SERVER");
            string msg = "REDANFE|" + chave + "|"+ comboBox1.SelectedValue +"|END|";
            string result = sendNfce(msg) ;
            
            string txt = "";

            if (result.Equals("|X|"))
                txt = "Documento enviado para Impressora";
            else
                txt = result;
                
            MessageBox.Show(txt,"Impressao de Documento", MessageBoxButtons.OK,MessageBoxIcon.Information);
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
        private void getWSID()
        {
            
            
            System.IO.StreamReader file = new System.IO.StreamReader(@"config_nfce_sat.cfg");
            while (!file.EndOfStream)
            {
                string line = file.ReadLine() ;
                if (line == "[WSID / IP]")
                {
                   while ((line = file.ReadLine()) != null)
                   {
                       if (line.Equals("[RODAPE]"))
                           return;
                       if(!String.IsNullOrEmpty(line))
                       {
                           string[] words = line.Split('=');
                           listImpressora.Add(words[0], words[1]);
                       }
                   }
                }
                
            }
            file.Close();
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
                        if (word.Equals(_key))
                            return words[1];
                    }
                }
            }
            file.Close();
            return "";
        }
    }
}
