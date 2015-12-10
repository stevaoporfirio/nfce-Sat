using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace invoiceServerApp
{

    class managerData
    {
        private int PageNumber = 1;
        nfce_dados dados = new nfce_dados();
        DateTime AtualDate = new DateTime();
        public managerData()
        { 
        }
        public void consultaDados(DateTime _data)
        {
            PageNumber = 1;
            AtualDate = _data;
            DataTable dt = ManagerDB.Instance().SelectNotasStatus(PageNumber, _data);
            dados = new nfce_dados();
            dados.setDataTable(dt);
        }
        public DataTable getDataTable(int _page, DateTime _data)
        {
            return dados.getDados();
        }
        public DataTable getstatus(DateTime _data, int _id)
        {
            return dados.getstatus(_id);
        }
        public DataTable getDados(DateTime _data, int _id)
        {
            return dados.getDadosById(_id);
        }
        public DataTable getNextData(DateTime _data)
        {
            DataTable dt = null;
            if (AtualDate == _data)
            {
                pagechanged(1);
                dt = ManagerDB.Instance().SelectNotasStatus(PageNumber, _data);
                dados.setDataTable(dt);
                return dt;
                
            }
            else
            {
                consultaDados(_data);
            }

            return dt;
            //return getDados(_data, PageNumber);
        }
        public DataTable getBackData(DateTime _data)
        {
            if (AtualDate == _data)
            {
                pagechanged(0);
                DataTable dt = ManagerDB.Instance().SelectNotasStatus(PageNumber, _data);
                dados.setDataTable(dt);
                return dt;
            }
            else
            {
                consultaDados(_data);
            }

            return getDados(_data,PageNumber);
        }
        private void pagechanged(int _number)
        {
            if (_number == 1)
                PageNumber += 50;
            else
                PageNumber -= 50;
            if (PageNumber < 0)
                PageNumber = 1;
        
        }
    }
}
