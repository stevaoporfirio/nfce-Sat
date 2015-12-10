using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace invoiceServerApp
{
    public class ManagerDB
    {
        private SatServerAppDBMaster DB;
        private static readonly ManagerDB instance = new ManagerDB();

        private int tipoDB;

        private object locker = new object();

        public void SetDBMicros(int tipo)
        {
            tipoDB = tipo;

            if (tipoDB == 0)
                DB = new MicrosDB("micros", "custom", "custom");
            else if (tipoDB == 1)
                DB = new SimpDB("datastoredb", "datastoredb");
            else
                throw new Exception("Tipo de Banco Definido não existe. (0 - Micros | 1 - Simphony)");
        }

        public static ManagerDB Instance()
        {            
            return instance;            
        }
        public DataTable selectDadosNfce(string _id)
        {
            return DB.SendSelectMultiResultQueries(String.Format("SELECT  id, chave, nprot FROM custom.nfce_dados WHERE (id = {0})", _id), "status"); ;
        }

        public string SelectMaxNFCE(int _serie)
        {
            lock (locker)
            {
                string query = String.Format("select isnull(max(numero),0) from custom.nfce_dados where (serie = {0})", _serie);
                return DB.SendSelectOneResultQueries(query, false);
            }
        }
        public string SelectNumeroRecibo(string _id)
        {
            lock (locker)
            {
                string query = String.Format("select recibo from custom.nfce_dados where (id = {0})", Convert.ToInt32(_id));
                return DB.SendSelectOneResultQueries(query, false);
            }
        }
        public string SelectMaxNFCEidDB(string _serie, string _seq)
        {
            lock (locker)
            {
                string query = String.Format("select isnull(max(id),0) from custom.nfce_dados where (serie = {0} and chave = '{1}')", Convert.ToInt32(_serie), _seq);
                return DB.SendSelectOneResultQueries(query, false);
            }
        }
   
        public void InsertNfceInitial(string seq, string serie, string chk_num, string ws_id, string chaveAcesso)
        {
            lock (locker)
            {

                string query = String.Format("insert into custom.nfce_dados (numero, serie, check_id, ws_id, chave, data) values ({0},{1},{2},'{3}','{4}','{5}')"
                       , seq, serie, chk_num, ws_id, chaveAcesso, DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));

                DB.SendInsertQueries(query);
            }
        }

        public string InsertNfceInitial(string seq, string serie, string chk_num, string ws_id)
        {
            lock (locker)
            {
                int n = Convert.ToInt32(SelectMaxNFCE(Convert.ToInt32(serie)));

                n++;

                string query = String.Format("insert into custom.nfce_dados (numero, serie, check_id, ws_id, data) values ({0},{1},{2},'{3}','{4}')"
                       , n, serie, chk_num, ws_id, DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));

                DB.SendInsertQueries(query);

                return n.ToString();
            }
        }

        public void UpdateChaveDeAcesso(string n, string s, string chave)
        {
            lock (locker)
            {
                string query = String.Format("update custom.nfce_dados set chave = '{0}' where numero = '{1}' and serie = '{2}' ", chave, n, s);
                DB.SendInsertQueries(query);
            }
        }

        public void InsertNfceStatus(string Id_db, int status, string status_desc, string Info )
        {
            lock (locker)
            {
                string query = String.Format("insert into custom.nfce_status   (id_dados, nfce_status, nfce_status_desc, nfce_info, nfce_data) values ({0},{1},'{2}','{3}','{4}')"
                       , Id_db, status, status_desc, Info.Replace(Convert.ToChar(39), Convert.ToChar(28)), DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));

                DB.SendInsertQueries(query);
            }
        }
        public void UpdatenProtNFCe(string Id_db, string _nprot)
        {
            lock (locker)
            {
                string query = String.Format("update custom.nfce_dados set nprot = '{0}' where id = {1}", _nprot, Id_db);
                DB.SendInsertQueries(query);
            }

        }

        public void UpdateReciboNFCe(string Id_db, string recibo)
        {
            lock (locker)
            {
                string query = String.Format("update custom.nfce_dados set recibo = '{0}' where id = {1}", recibo, Id_db);
                DB.SendInsertQueries(query);
            }

        }
        public DataTable SelectNotaConta(int _num)
        {
            lock (locker)
            {
                string query = String.Format("select id, chave from custom.nfce_dados d where check_id = {0}", _num);
                return DB.SendSelectMultiResultQueries(query, "dados");
            }
        }
        public DataTable SelectNotaChave(string _num)
        {
            lock (locker)
            {
                string query = String.Format("select id, chave from custom.nfce_dados d where chave = '{0}'", _num);
                return DB.SendSelectMultiResultQueries(query, "dados");
            }
        }

        public DataTable SelectNotasStatus(int _num, DateTime _data)
        {
            //string query = String.Format("select TOP 50 START AT {0} d.*,  (select isnull(MAX(nfce_status),0) from nfce_status where id_dados = d.id) status from custom.nfce_dados d where convert(char(20),data,'105') = '{1}' ORDER BY id desc", _num, _data.ToString("dd-MM-yyyy"));
            lock (locker)
            {
                return DB.SelectNotasStatus(_num, _data);
            }
            //return DB.SendSelectMultiResultQueries(query,"dados");
        }
        public DataTable SelectStatusNotas(string _ids)
        {
            lock (locker)
            {
                return DB.SendSelectMultiResultQueries(String.Format("select id_dados, nfce_status, nfce_status_desc, nfce_data, isnull(nfce_info,'') nfce_info from custom.nfce_status where (id_dados in  ({0}))", _ids), "status");
            }
        }
        public DataTable RelatorioNFCE(DateTime _data1, DateTime _data2)
        {
            lock (locker)
            {
                string query = String.Format("SELECT d.*, (select isnull(MAX(nfce_status),0) from custom.nfce_status where id_dados = d.id) as status FROM custom.nfce_dados d WHERE (CONVERT(CHAR(20), data, 102) BETWEEN '{0}' AND '{1}')", _data1.ToString("yyyy.MM.dd"), _data2.ToString("yyyy.MM.dd"));
                return DB.SendSelectMultiResultQueries(query, "relatorio");
            }
        }
        public DataSet SelectNFCE(DateTime d1, DateTime d2)
        {
            lock (locker)
            {
                try
                {                
                    DataSet ds = new DataSet();
                    DataTable dt = null;

                    dt = DB.SendSelectMultiResultQueries(
                        String.Format("select id, numero, serie, check_id, ws_id, chave, isnull(recibo,'') recibo , data from custom.nfce_dados where convert(char(20),data,'105') between '{0}' and '{1}' order by 8 desc"
                        , d1.ToString("dd-MM-yyyy")
                        , d2.ToString("dd-MM-yyy")
                        ), "dados");



                    ds.Merge(dt);

                    dt = DB.SendSelectMultiResultQueries(String.Format("select id_dados, nfce_status, nfce_status_desc, nfce_data, isnull(nfce_info,'') nfce_info from nfce_status where convert(char(20),nfce_data,'105') between  '{0}' and '{1}' order by 1,4,2 desc"
                        , d1.ToString("dd-MM-yyyy")
                        , d2.ToString("dd-MM-yyy")
                        ), "status");
                
                    ds.Merge(dt);

                    return ds;
                
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
        } 
        
    }
}
