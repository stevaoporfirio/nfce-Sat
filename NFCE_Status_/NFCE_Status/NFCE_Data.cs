using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFCE_Status
{
    public class NFCE_Data
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Serie { get; set; }
        public int Chk_ID { get; set; }
        public int WS_ID { get; set; }
        public string NFCE_Key { get; set; }        
        public string NFCE_Recibo { get; set; }
        public DateTime NFCE_DateTime { get; set; }
        
        public int Status { get; set; }
        public string StatusDesc { get; set; }

        public string Info { get; set; }
        public DateTime NFCE_DateTimeCont { get; set; }

        public List<NFCE_Data_status> Status_List { get; set; }
        
    }

    public class NFCE_Data_status
    {
        public int NFCE_status{ get; set; }
        public string NFCE_status_desc { get; set; }
        public DateTime NFCE_status_Data { get; set; }
        public string Info { get; set; }
    }
}
