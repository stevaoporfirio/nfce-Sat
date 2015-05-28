using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace invoiceServerApp
{
    class nfce_status
    {
        private DataTable DtStatus;
        private int Status = 0;
        private int id = 0;
        public nfce_status(DataTable _status, int _id)
        {
            id = _id;
            DtStatus = _status;
        }
        public DataTable getStatus()
        {
            return DtStatus;
        }

        public void addRows(DataRow _row)
        {
            DtStatus.ImportRow(_row);
        }
    }
}
