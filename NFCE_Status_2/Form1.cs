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
        private managerData manager = new managerData();
        public Form1()
        {
            InitializeComponent();

            ManagerDB.Instance().SetDBMicros(Program.argDB);
        }

        private void btn_visualizar_Click(object sender, EventArgs e)
        {
            manager.consultaDados(dateTimePicker1.Value);
            DataTable dt = manager.getDataTable(0,dateTimePicker1.Value);
            Dtv_principal.DataSource = dt;

            Dtv_principal.Columns[0].Width = 75;
            Dtv_principal.Columns[1].Width = 100;
            Dtv_principal.Columns[2].Width = 50;
            Dtv_principal.Columns[3].Width = 100;
            Dtv_principal.Columns[4].Width = 50;

            Dtv_principal.Columns[5].Visible = false;
            Dtv_principal.Columns[6].Visible = false;

            Dtv_principal.Columns[7].Width = 175;

            Dtv_principal.Columns[8].Visible = false;

            //Dtv_principal.Columns[0].Width = 100;
        }

        private void Dtv_principal_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.Dtv_principal.Rows)
            {
                switch (Convert.ToInt32(row.Cells["status"].Value))
                {
                    case 12:
                        row.DefaultCellStyle.BackColor = Color.Green;
                        break;
                    case 16:
                        row.DefaultCellStyle.BackColor = Color.Red;
                        break;
                    case 19:
                        row.DefaultCellStyle.BackColor = Color.PowderBlue;
                        break;
                    case 7:
                        row.DefaultCellStyle.BackColor = Color.Pink;
                        break;

                }
                Dtv_principal.Refresh();

            }
        }

        private void Dtv_principal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int num = Convert.ToInt32(Dtv_principal.Rows[e.RowIndex].Cells[0].Value.ToString());

            DataTable dt_dados = manager.getDados(dateTimePicker1.Value, num);
            DataTable dt_status = manager.getstatus(dateTimePicker1.Value, num);
            FormStatus f = new FormStatus();
            f.setDatatableStatus(dt_status);
            f.setDatatableDados(dt_dados);
            f.MontaForm();

            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                button2.Enabled = false;
                DataTable dt = manager.getNextData(dateTimePicker1.Value);
                if (dt != null)
                    Dtv_principal.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                button2.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;

                DataTable dt = manager.getBackData(dateTimePicker1.Value);
                if (dt != null)
                    Dtv_principal.DataSource = dt;
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                button1.Enabled = true;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Consultar cons = new Consultar();
            cons.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Relatorio a = new Relatorio();
            a.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
