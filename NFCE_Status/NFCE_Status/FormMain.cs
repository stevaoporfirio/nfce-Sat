using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFCE_Status
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonVisualizar_Click(object sender, EventArgs e)
        {
            Program.appControle = null;
                
            DoForm();

        }

        private void DoForm()
        {

            Program.appControle = new AppControle(dateTimePicker1.Value, dateTimePicker2.Value);
            int filtro = 0;

            checkedListBox1.Enabled = true;
            buttonExportar.Enabled = true;

            //filtrando usando potencias de 2
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    if (i == 0)
                        filtro += i + 1;
                    else
                        filtro += Convert.ToInt32(Math.Pow(2, i));
                }

            }

            dataGridView1.DataSource = Program.appControle.GetDataSet(filtro);

            //dataGridView1.DataMember = "NFCE";

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;

            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[4].Width = 60;
            dataGridView1.Columns[6].Width = 400;


            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                //r.Height = 35;

                string status = r.Cells[5].Value.ToString();

                if (status.Equals("12"))
                    r.DefaultCellStyle.BackColor = Color.LightGreen;
                else if (status.Equals("13"))
                    r.DefaultCellStyle.BackColor = Color.LightGreen;
                else if (status.Equals("7"))
                    r.DefaultCellStyle.BackColor = Color.Tomato;
                else if (status.Equals("10"))
                    r.DefaultCellStyle.BackColor = Color.Tomato;
                else if (status.Equals("8"))
                    r.DefaultCellStyle.BackColor = Color.Yellow;

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int numero = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());       

            NFCE_Data nf = Program.appControle.GetNFCE(numero);

            Form1 f = new Form1(nf,numero);


            f.ShowDialog();

            if (f.DialogResult == DialogResult.OK)
            {
                DoForm();
            }
        }

        private void buttonExportar_Click(object sender, EventArgs e)
        {
            Program.appControle.SetRelatorio();

            MessageBox.Show("Arquivo Exportado com Sucesso");
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Program.appControle = null;
            checkedListBox1.Enabled = false;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            Program.appControle = null;
            checkedListBox1.Enabled = false;
        }
    }
}

