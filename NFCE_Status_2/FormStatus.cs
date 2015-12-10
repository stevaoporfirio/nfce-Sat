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
    public partial class FormStatus : Form
    {
        DataTable dt_status;
        DataTable dt_dados;
        string id = "";
        string numero = "";

        public FormStatus()
        {
            InitializeComponent();
        }
        public void MontaForm()
        {
            foreach (DataRow row in dt_dados.Rows) 
            {
                txtChaveAcesso.Text = row["chave"].ToString();
                txtDtEmissao.Text = row["data"].ToString();
                txtNumConta.Text = row["check_id"].ToString();
                txtNumNFCe.Text = row["numero"].ToString();
                txtStatusNFCe.Text = row["status"].ToString();
                txtWSID.Text = row["ws_id"].ToString();
                txtNumRecibo.Text = row["recibo"].ToString();
                id = row["id"].ToString();
                numero = row["numero"].ToString();
                    
            }

            DataRow[] dtRows = dt_status.Select(string.Format("nfce_status = '{0}' or nfce_status = '{1}' ", "12", "13"));
            if (dtRows.Count() == 1)
            {
                DataRow[] dtRowCanceladas = dt_status.Select(string.Format("nfce_status = '{0}'", "16"));
                if (dtRowCanceladas.Count() < 1)
                    btnCancelar.Enabled = true;
            }
            else
            {
                DataRow[] dtRowCanceladas = dt_status.Select(string.Format("nfce_status = '{0}'", "19"));
                if (dtRowCanceladas.Count() < 1)
                    btnInutilizar.Enabled = true;
            }   
                

            dataGridView1.DataSource = dt_status;
            dataGridView1.Refresh();
        }
        public void setDatatableStatus(DataTable _dt)
        {
            dt_status = _dt;
        
        }
        public void setDatatableDados(DataTable _dt)
        {
            dt_dados = _dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ret = "";

            if (Program.argDB == 1)
            {
                //string msg = "CANCEL|" + id + "|END|";
                ret = "Cancelamento de SAT deverá ser realizado diretamente na operação.\n(Micros 3700 / Simphony)";
            }
            else
            {
                string msg = "CANCEL|" + id + "|END|";
                ret = sendNfce(msg);
            }
            
            MessageBox.Show(ret);
        }

        private void btnInutilizar_Click(object sender, EventArgs e)
        {
            string msg = "INUTILIZACAO|" + id + "|" + numero + "|END|";
            MessageBox.Show(sendNfce(msg));
            DialogResult = DialogResult.OK;

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

        private void button1_Click(object sender, EventArgs e)
        {
            string chave = txtChaveAcesso.Text;

            //string wsid = getCFGConfig("IP_SERVER");
            string printer = getCFGConfig(txtWSID.Text);

            string msg = "REDANFE|" + chave + "|" + printer + "|END|";
            string result = sendNfce(msg);

            string txt = "";

            if (result.Equals("|X|"))
                txt = "Documento enviado para Impressora";
            else
                txt = result;

            MessageBox.Show(txt, "Impressao de Documento", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void FormStatus_Load(object sender, EventArgs e)
        {
            //button2.Enabled = false;
            //getWSID();
            //comboBox1.DataSource = new BindingSource(listImpressora, null);
            //comboBox1.DisplayMember = "Key";
            //comboBox1.ValueMember = "Value";
        }
    }
}
