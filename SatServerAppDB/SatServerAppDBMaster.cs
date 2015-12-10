using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace invoiceServerApp
{
    public abstract class SatServerAppDBMaster
    {
        public abstract void SendInsertQueries(string q);

        public abstract DataTable SendSelectMultiResultQueries(string q, string table);

        public abstract string SendSelectOneResultQueries(string q, bool erroOnNull);

        public abstract DataTable SelectNotasStatus(int _num, DateTime _data);        
    }
}
