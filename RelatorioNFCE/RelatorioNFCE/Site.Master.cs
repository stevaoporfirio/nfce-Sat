using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RelatorioNFCE
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public AppControle controller;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
         //   controller = new AppControle(Calendar2.SelectedDate, Calendar3.SelectedDate, 0);
        }
    }
}
