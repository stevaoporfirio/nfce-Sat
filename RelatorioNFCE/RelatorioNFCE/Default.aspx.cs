using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace RelatorioNFCE
{
    public partial class _Default : System.Web.UI.Page
    {
        private DataTable dt;
        public AppControle controller;
        protected void Page_Load(object sender, EventArgs e)
        {


            //ModalPopupExtender1.Hide();

        }

        protected void DataList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            controller = new AppControle(Calendar1.SelectedDate, Calendar2.SelectedDate);
            dt = controller.GetDataSet(0);
            GridView1.DataSource = dt;


            GridView1.DataMember = "NFCE_Filtro";

            
            GridView1.DataBind();
                        
            int a = GridView1.Columns.Count;
            
        }
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnsubmit = sender as LinkButton;
                GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
                string a = gRow.Cells[2].Text;
            }
            catch(Exception ex)
            {
                
                return;
            }
            finally
            {
              
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                e.Row.Cells[0].Width = 70;
                e.Row.Cells[1].Width = 70;
                e.Row.Cells[2].Width = 70;
                e.Row.Cells[3].Width = 70;
                e.Row.Cells[4].Width = 70;
                e.Row.Cells[5].Width = 70;
                e.Row.Cells[6].Width = 70;
                e.Row.Cells[7].Width = 200;
                e.Row.HorizontalAlign = HorizontalAlign.Left;
                
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow rowSelect = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;  
        }
    }
}
