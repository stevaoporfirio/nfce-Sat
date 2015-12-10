using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using invoiceServerApp;

namespace NFCE_Status
{
    public class AppControle
    {
        private DataSet ds;
        private List<NFCE_Data> nfceList;
        private List<NFCE_Data> nfceListFiltro;
        private int filtro;

        private DateTime dataInicial;
        private DateTime dataFinal;
        public AppControle()
        {
            SelectNFCE();
        }
        public AppControle(DateTime di, DateTime df)
        {  

            dataInicial = di;
            dataFinal = df;
            //filtro = f;

            SelectNFCE();
        }

        public void SetRelatorio()
        {
            NfceList();

            List<NFCE_Data> nfceEnv = nfceList.FindAll(a => a.Status== 12);

            List<NFCE_Data> nfceRej = nfceList.FindAll(a => a.Status == 7);

            List<NFCE_Data> nfceCont = nfceList.FindAll(a => a.Status == 8);

            List<NFCE_Data> nfceEnvCont = nfceList.FindAll(a => a.Status == 13);

            List<NFCE_Data> nfceRejCont = nfceList.FindAll(a => a.Status == 10);


            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format("Relatorio NFCE [{0} a {1}]", dataInicial.ToString("dd-MM-yyyy"), dataFinal.ToString("dd-MM-yyyy")));

            sb.AppendLine(String.Format("{0,8}{1,8}{2,8}{3,8}{4,46}{5,8}{6,40}{7,35}{8,30}{9,256}{10,23}"
                , "Numero".PadRight(6)
                , "Serie".PadRight(6)
                , "Chk ID".PadRight(6)
                , "WS ID".PadRight(6)
                , "Chave NFE".PadRight(44)
                , "Status".PadRight(1)
                , "Descricao".PadRight(12)
                , "Recibo".PadRight(30)
                , "Data/Hora Envio".PadRight(23)
                , "Informacao (Rejeicao)".PadRight(256)
                , "Data/Hora Envio Contingencia".PadRight(23)
                ));

            foreach (NFCE_Data n in nfceEnv)
            {
                string line = (String.Format("{0,8}{1,8}{2,8}{3,8}{4,46}{5,8}{6,40}{7,35}{8,30}"
                    , n.Numero.ToString().PadRight(6)
                    , n.Serie.ToString().PadRight(6)
                    , n.Chk_ID.ToString().PadRight(6)
                    , n.WS_ID.ToString().PadRight(6)
                    , n.NFCE_Key.ToString().PadRight(44)
                    , n.Status.ToString().PadRight(2)
                    , n.StatusDesc.ToString().PadRight(35)
                    , n.NFCE_Recibo.ToString().PadRight(30)
                    , n.NFCE_DateTime.ToString().PadRight(23)
                    ));

                sb.AppendLine(line);
            }

            foreach (NFCE_Data n in nfceRej)
            {
                string line = (String.Format("{0,8}{1,8}{2,8}{3,8}{4,46}{5,8}{6,40}{7,35}{8,30}{9,256}"
                    , n.Numero.ToString().PadRight(6)
                    , n.Serie.ToString().PadRight(6)
                    , n.Chk_ID.ToString().PadRight(6)
                    , n.WS_ID.ToString().PadRight(6)
                    , n.NFCE_Key.ToString().PadRight(44)
                    , n.Status.ToString().PadRight(2)
                    , n.StatusDesc.ToString().PadRight(35)
                    , n.NFCE_Recibo.ToString().PadRight(30)
                    , n.NFCE_DateTime.ToString().PadRight(23)
                    , n.Info.PadRight(256)
                    ));

                sb.AppendLine(line);
            }

            foreach (NFCE_Data n in nfceCont)
            {
                string line = (String.Format("{0,8}{1,8}{2,8}{3,8}{4,46}{5,8}{6,40}{7,35}{8,30}"
                    , n.Numero.ToString().PadRight(6)
                    , n.Serie.ToString().PadRight(6)
                    , n.Chk_ID.ToString().PadRight(6)
                    , n.WS_ID.ToString().PadRight(6)
                    , n.NFCE_Key.ToString().PadRight(44)
                    , n.Status.ToString().PadRight(2)
                    , n.StatusDesc.ToString().PadRight(35)
                    , n.NFCE_Recibo.ToString().PadRight(30)
                    , n.NFCE_DateTime.ToString().PadRight(23)
                    ));

                sb.AppendLine(line);
            }

            foreach (NFCE_Data n in nfceEnvCont)
            {
                string line = (String.Format("{0,8}{1,8}{2,8}{3,8}{4,46}{5,8}{6,40}{7,35}{8,30}{9,256}{10,23}"
                    , n.Numero.ToString().PadRight(6)
                    , n.Serie.ToString().PadRight(6)
                    , n.Chk_ID.ToString().PadRight(6)
                    , n.WS_ID.ToString().PadRight(6)
                    , n.NFCE_Key.ToString().PadRight(44)
                    , n.Status.ToString().PadRight(2)
                    , n.StatusDesc.ToString().PadRight(35)
                    , n.NFCE_Recibo.ToString().PadRight(30)
                    , n.NFCE_DateTime.ToString().PadRight(23)
                    , n.Info.PadRight(256)
                    , n.NFCE_DateTimeCont
                    ));

                sb.AppendLine(line);
            }

            foreach (NFCE_Data n in nfceRejCont)
            {
                string line = (String.Format("{0,8}{1,8}{2,8}{3,8}{4,46}{5,8}{6,40}{7,35}{8,30}{9,256}{10,23}"
                    , n.Numero.ToString().PadRight(6)
                    , n.Serie.ToString().PadRight(6)
                    , n.Chk_ID.ToString().PadRight(6)
                    , n.WS_ID.ToString().PadRight(6)
                    , n.NFCE_Key.ToString().PadRight(44)
                    , n.Status.ToString().PadRight(2)
                    , n.StatusDesc.ToString().PadRight(39)
                    , n.NFCE_Recibo.ToString().PadRight(30)
                    , n.NFCE_DateTime.ToString().PadRight(23)
                    , n.Info.PadRight(256)
                    , n.NFCE_DateTimeCont
                    ));

                sb.AppendLine(line);
            }

            sb.AppendLine(String.Format("Totais:"));
            sb.AppendLine(String.Format("{0,24}:\t{1,-5}", "Enviadas", nfceEnv.Count));
            sb.AppendLine(String.Format("{0,24}:\t{1,-5}", "Rejeitadas", nfceRej.Count));
            sb.AppendLine(String.Format("{0,24}:\t{1,-5}", "Fila Contingencia", nfceCont.Count));
            sb.AppendLine(String.Format("{0,24}:\t{1,-5}", "Enviadas Contingencia", nfceEnvCont.Count));
            sb.AppendLine(String.Format("{0,24}:\t{1,-5}", "Rejeitadas Contingencia", nfceRejCont.Count));
            sb.AppendLine(String.Format("{0,24}:\t{1,-5}", "Total", nfceList.Count));

            string fileName = String.Format("RelatorioNFCE_{0}_{1}.txt", dataInicial.ToString("ddMMyyyy"), dataFinal.ToString("ddMMyyyy"));
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(sb);
                sw.Flush();
                sw.Close();
            }
        }

        public NFCE_Data GetNFCE(int numero)
        {
            //NfceList();

            return nfceList.Single(a => a.Id == numero);

        }

        public DataTable GetDataSet(int f)        
        {
            filtro = f;

            DataTable dtFiltro = new DataTable("NFCE");

            DataColumn dc = null;

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "ID";
            dc.Caption = "ID";
            dtFiltro.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Num";
            dc.Caption = "Num";
            dtFiltro.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Serie";
            dc.Caption = "Serie";
            dtFiltro.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Check ID";
            dc.Caption = "Check ID";
            dtFiltro.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "WS ID";
            dc.Caption = "WS ID";
            dtFiltro.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Status Num";
            dc.Caption = "Status";
            dtFiltro.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Status";
            dc.Caption = "Status";
            dtFiltro.Columns.Add(dc);
 
            

            NfceList();

            List<NFCE_Data> tmpList = new List<NFCE_Data>();
            if (f > 0)
                tmpList = nfceListFiltro;
            else
                tmpList = nfceList;

            foreach (NFCE_Data n in tmpList)
            {
                DataRow dr = dtFiltro.NewRow();
                
                dr[0] = n.Id;
                dr[1] = n.Numero;
                dr[2] = n.Serie;
                dr[3] = n.Chk_ID;
                dr[4] = n.WS_ID;
                dr[5] = n.Status.ToString();
                dr[6] = n.StatusDesc.ToString();

                dtFiltro.Rows.Add(dr);
            }

            //ds.Merge(dtFiltro);

            return dtFiltro;
        }
        
        public List<NFCE_Data> GetLista()
        {

            NfceList();

            return nfceList;    
        }

        public void SelectNFCE()
        {
            ds = new DataSet();

            //string tmpQ = String.Format("select seq, number,check_id, ws_id, nfce_key, nfce_status, nfce_status_desc, isnull(nfce_recibo,'')nfce_recibo, nfce_datetime, isnull(cast(nfce_datetime_cont as char(21)),'')nfce_datetime_cont, isnull(info,'')info from nfce_data"
            //+ " where convert(char(20),NFCE_DateTime,'105') between '{0}' and '{1}'"
            //    , dataInicial.ToString("dd-MM-yyyy")
            //    , dataFinal.ToString("dd-MM-yyyy")
            //    );

            //ds.Merge(ManagerDB.Instance(config.configMaquina.TipoDB). .SendSelectMultiResultQueries(tmpQ, "NFCE"));            

            ds = ManagerDB.Instance(0).SelectNFCE(dataInicial,dataFinal);

        }

        private void NfceList()
        {
            nfceList = new List<NFCE_Data>();            

            //SelectNFCE();

            var queryNFCE = from nfce in ds.Tables["DADOS"].AsEnumerable()
                            select new
                            {
                                ID= nfce.Field<Int32>("id"),
                                Numero = nfce.Field<Int32>("numero"),
                                Serie = nfce.Field<Int32>("serie"),
                                Check_ID = nfce.Field<Int32>("check_id"),
                                WS_ID  = nfce.Field<Int32>("ws_id"),
                                NFCE_Key  = nfce.Field<string>("chave"),                                
                                NFCE_Recibo  = nfce.Field<string>("recibo"),
                                NFCE_DateTime  = nfce.Field<DateTime>("data")                                
                            };

            foreach(var n in queryNFCE)
            {
                NFCE_Data nd = new NFCE_Data();
                nd.Id = n.ID;
                nd.Numero = n.Numero;
                nd.Serie = n.Serie;
                nd.Chk_ID = n.Check_ID;
                nd.WS_ID = n.WS_ID;
                nd.NFCE_Key = n.NFCE_Key;                
                nd.NFCE_Recibo = n.NFCE_Recibo;
                nd.NFCE_DateTime = n.NFCE_DateTime;
                

                var queryStatus = from nfce in ds.Tables["STATUS"].AsEnumerable()
                                  where nfce.Field<Int32>("id_dados") == n.ID
                                  select new
                                  {
                                      NFCE_status = nfce.Field<int>("nfce_status"),
                                      NFCE_status_desc = nfce.Field<string>("nfce_status_desc"),
                                      NFCE_DateTime_data = nfce.Field<DateTime>("nfce_data"),
                                      Info = nfce.Field<string>("nfce_info")
                                  };
                nd.Status_List = new List<NFCE_Data_status>();

                foreach (var s in queryStatus)
                {
                    NFCE_Data_status nds = new NFCE_Data_status();

                    nds.NFCE_status = s.NFCE_status;
                    nds.NFCE_status_desc = s.NFCE_status_desc;
                    nds.NFCE_status_Data = s.NFCE_DateTime_data;
                    nds.Info = s.Info;

                    nd.Status_List.Add(nds);
                }

                if (queryStatus.Count() > 0)
                {
                    nd.Status = queryStatus.LastOrDefault().NFCE_status;
                    nd.StatusDesc = queryStatus.LastOrDefault().NFCE_status_desc;

                    nd.Info = queryStatus.LastOrDefault().Info;
                    nd.NFCE_DateTimeCont = queryStatus.LastOrDefault().NFCE_DateTime_data;

                    nfceList.Add(nd);
                }
            }

            nfceListFiltro = new List<NFCE_Data>();

            if (filtro == 0)
                nfceListFiltro = nfceList;
            else
            {

                while (filtro != 0)
                {
                    if (filtro >= 16)
                    {
                        nfceListFiltro.AddRange(nfceList.FindAll(a => a.Status == 10)); //Rej Con
                        filtro -= 16;

                    }
                    else if (filtro >= 8)
                    {
                        nfceListFiltro.AddRange(nfceList.FindAll(a => a.Status == 13)); //OK Con
                        filtro -= 8;
                    }
                    else if (filtro >= 4)
                    {
                        nfceListFiltro.AddRange(nfceList.FindAll(a => a.Status == 8)); //Fila
                        filtro -= 4;
                    }
                    else if (filtro >= 2)
                    {
                        nfceListFiltro.AddRange(nfceList.FindAll(a => a.Status == 7)); //Rej
                        filtro -= 2;
                    }
                    else if (filtro >= 1)
                    {
                        nfceListFiltro.AddRange(nfceList.FindAll(a => a.Status == 12)); //OK
                        filtro -= 1;
                    }
                }


                List<NFCE_Data> tmpList = nfceListFiltro.OrderByDescending(a => a.Numero).ToList();

                nfceListFiltro = nfceListFiltro.OrderByDescending(a => a.Numero).ToList();
            } 
        }
    }
}


