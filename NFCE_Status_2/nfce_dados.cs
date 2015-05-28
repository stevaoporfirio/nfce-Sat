using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace invoiceServerApp
{
    class nfce_dados
    {
        private Dictionary<int, nfce_status> mapStatus = new Dictionary<int, nfce_status>();
        private DataTable dtDados;
        public nfce_dados()
        {

        }
        public void setDataTable(DataTable _dt)
        {
            if (_dt.Rows.Count > 0)
            {
                dtDados = _dt;
                processa();
            }
        }
        public DataTable getDados()
        {
            return dtDados;
        }

        public DataTable getstatus(int _id)
        {
            if (mapStatus.ContainsKey(_id))
                return mapStatus[_id].getStatus();
            return null;
        }
        public DataTable getDadosById(int _id)
        {
            
            DataRow[] dtRows = dtDados.Select(string.Format("id = {0} ", _id));
            if (dtRows.Count() > 0)
                return dtRows.CopyToDataTable();
            else
                return null;
        
        }

        private void processa()
        {
            if (dtDados.Rows.Count != 0)
            {
                string ids = "";
                foreach (DataRow row in dtDados.Rows)
                {
                    if (row["id"].ToString() != null)
                        ids += row["id"].ToString() + ", ";
                }
                ids = ids.Substring(0, ids.Length - 2);

                DataTable dtStatus = ManagerDB.Instance.SelectStatusNotas(ids);

                foreach (DataRow row in dtStatus.Rows)
                {
                    if (row["id_dados"].ToString() != "")
                    {
                        int id = Convert.ToInt32(row["id_dados"].ToString());

                        if (!mapStatus.ContainsKey(id))
                        {
                            DataTable dt = dtStatus.Clone();
                            nfce_status status = new nfce_status(dt, id);
                            status.addRows(row);
                            mapStatus.Add(id, status);
                        }
                        else
                        {
                            mapStatus[id].addRows(row);
                        }
                    }

                }

            }
        }
    }
}
