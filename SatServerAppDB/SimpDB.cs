using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace invoiceServerApp
{
    public class SimpDB : SatServerAppDBMaster
    {        
        private string user;
        private string pass;

        private SqlConnection con;
                
        public List<string> queryLines { get; set; }

        public SimpDB(string _user, string _pass)
        {
            try
            {

                user = _user;
                pass = _pass;

                queryLines = new List<string>();

                string server = null;

#if DEBUG
                server = "172.35.207.200\\SQLEXPRESS";
#else
                server = "localhost\\SQLEXPRESS";
#endif

                con = new SqlConnection(String.Format("user id={0};" +
                                           "password={1};server={2};" +
                                           "Trusted_Connection=no;" +
                                           "database={3}; " +
                                           "connection timeout=30"
                                           , _user, _pass, server, "DataStore"
                                           ));

                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro abrindo comunicacao DB\n" + server + "DataStore");
                }

                //criando tabela se não existir

                try
                {

                    string q1 = String.Format(
                        " IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[nfce_dados]') AND type in (N'U')) " + 
                        " CREATE TABLE [dbo].[nfce_dados]( [id] [int] IDENTITY(1,1) NOT NULL, [numero] [int] NULL, [serie] [int] NULL, [check_id] [int] NULL, [ws_id] [int] NULL, [chave] [varchar](50) NULL, [recibo] [varchar](50) NULL, [data] [datetime] NULL, [nprot] [varchar](50) NULL, CONSTRAINT [PK_nfce_dados] PRIMARY KEY CLUSTERED  ( [id] ASC )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] ) ON [PRIMARY]" +

                        " IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[nfce_status]') AND type in (N'U')) " +
                        " CREATE TABLE [dbo].[nfce_status]( [id] [int] IDENTITY(1,1) NOT NULL, [id_dados] [int] NOT NULL, [nfce_status] [int] NULL, [nfce_status_desc] [varchar](512) NULL, [nfce_data] [datetime] NULL, [nfce_info] [varchar](512) NULL ) ON [PRIMARY]  " +

                        " IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[nfce_status]') AND type in (N'U')) " +
                        " ALTER TABLE [dbo].[nfce_status]  WITH CHECK ADD  CONSTRAINT [FK_nfce_status_nfce_dados] FOREIGN KEY([id_dados]) REFERENCES [dbo].[nfce_dados] ([id])  "+

                        " IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[nfce_status]') AND type in (N'U')) " +
                        " ALTER TABLE [dbo].[nfce_status] CHECK CONSTRAINT [FK_nfce_status_nfce_dados]  "


                        );

                    SqlCommand com = new SqlCommand(q1, con);
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("#Erro Criando tabelas Simphony#\n" + ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void SendInsertQueries(string q)
        {
            try
            {
                q = q.Replace("custom", "DataStore.dbo");

                SqlCommand com = new SqlCommand(q, con);
                int i = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Erro inserindo dados no DB\n{0}\n{1}", q, ex.Message));
            }
        }

        public override DataTable SendSelectMultiResultQueries(string q, string table)
        {
            try
            {
                q = q.Replace("custom", "DataStore.dbo");

                using (SqlCommand com = new SqlCommand(q, con))
                {

                    using (SqlDataAdapter da = new SqlDataAdapter(com))
                    {

                        DataSet ds = new DataSet();

                        da.Fill(ds, table);
                        DataTable microsDT = ds.Tables[table];

                        return microsDT;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string SendSelectOneResultQueries(string q, bool erroOnNull)
        {
            try
            {
                q = q.Replace("custom", "DataStore.dbo");

                using (SqlCommand com = new SqlCommand(q, con))
                {

                    using (SqlDataAdapter da = new SqlDataAdapter(com))
                    {

                        DataSet ds = new DataSet();

                        da.Fill(ds, "tmp");
                        DataTable microsDT = ds.Tables["tmp"];

                        return microsDT.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataTable SelectNotasStatus(int _num, DateTime _data)
        {
            //

            string query = String.Format("WITH teste AS " +
            " (" +
                " SELECT *," +
                " indice = ROW_NUMBER() OVER (ORDER BY id) " +
                " FROM nfce_dados" +
            " )" +
            " SELECT *" +
            " ,(select isnull(MAX(nfce_status),0) from DataStore.dbo.nfce_status where id_dados = d.id) status "+
            " FROM nfce_dados d" +
            " WHERE  convert(char(20),data,105) = '{0}'" +
            " and id BETWEEN {1} AND {2}" +
            " ORDER BY id", _data.ToString("dd-MM-yyyy"), _num, _num + 50);

            return SendSelectMultiResultQueries(query, "dados");
        }
    }
}
