using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.IO;

namespace invoiceServerApp
{
    public class MicrosDB
    {
        private string odbcName;
        private string odbcUser;
        private string odbcPass;

        private OdbcConnection odbcMicros;
        private OdbcConnectionStringBuilder odbcBuilder;
        //public DataTable microsDT;

        public List<string> queryLines { get; set; }
        //private string queryCMD;

        public MicrosDB(string _odbcNome, string _odbcUser, string _odbcPass)
        {
            //microsDT = new DataTable();
            odbcName = _odbcNome;
            odbcUser = _odbcUser;
            odbcPass = _odbcPass;

            queryLines = new List<string>();

            string connectionString = "Dsn=" + odbcName
                       + ";UID=" + odbcUser
                       + ";PWD=" + odbcPass;

            //string sqlCommand = query;

            odbcMicros = new OdbcConnection(connectionString);
            odbcMicros.Open();
        }

        public void SendInsertQueries(string q)
        {
            try
            {                
                OdbcCommand com = new OdbcCommand(q, odbcMicros);
                int i = com.ExecuteNonQuery();
                //int nada = i;   

                //using (StreamWriter file = new StreamWriter(@"CheckMFD_Log.txt", true, Encoding.ASCII))
                //{
                //    file.WriteLine(String.Format("Querie: {0}\n\n Retorno: {1}\n", q, i));
                //    file.Close();
                //}
            }catch (Exception ex)
            {
                throw new Exception(String.Format("Erro inserindo dados no DB\n{0}\n{1}",q,ex.Message));
            }
        }

        public DataTable SendSelectMultiResultQueries(string q, string table)
        {
            try
            {
                OdbcCommand com = new OdbcCommand(q, odbcMicros);
                //OdbcDataReader ret = com.ExecuteReader();

                OdbcDataAdapter da = new OdbcDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds, table);
                DataTable microsDT = ds.Tables[table];

                return microsDT;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        public string SendSelectOneResultQueries(string q, bool erroOnNull)
        {
            try
            {                
                OdbcCommand com = new OdbcCommand(q, odbcMicros);
                OdbcDataReader ret = com.ExecuteReader();
                if (ret.HasRows)
                {
                    return ret[0].ToString();
                }
                else
                {
                    if (erroOnNull)
                        throw new Exception("Erro Executando Select\n" + q);
                    else
                        return "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
