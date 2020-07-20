using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfAppTest
{
    class Connection
    {
        string server;
        string port;
        string database;
        string username;
        string pass;
        MySqlConnection conn;
        MySqlCommand cmd;
        string time_format = "yyyy-MM-dd HH:mm:ss";

        public Connection()
        {
            // var myJsonString = File.ReadAllText("connection_settings.json");
            // dynamic jToken = JToken.Parse(myJsonString);
            server = "localhost";
            port = "3306";
            database = "ec38";
            username = "ec38";
            pass = "ec38";

            string connStr = "server=" + server + ";user=" + username + ";port=" + port + ";password=" + pass + ";";
            conn = new MySqlConnection(connStr);

            cmd = new MySqlCommand();
            cmd.Connection = conn;

            DropAndCreateNew();
            FillDatabase();
        }

        private void FillDatabase()
        {
            int dnid = InsertDelovniNalog("Delovni nalog 1", 2);
            InsertKos(123, dnid, DateTime.Now);
            InsertKos(321, dnid, DateTime.Now);

            dnid = InsertDelovniNalog("Delovni nalog 2", 1);
            InsertKos(2, dnid, DateTime.Now);

            dnid = InsertDelovniNalog("Delovni nalog 3", 1);
            InsertKos(3, dnid, DateTime.Now);

            dnid = InsertDelovniNalog("Delovni nalog 4", 1);
            InsertKos(4, dnid, DateTime.Now);

            dnid = InsertDelovniNalog("Delovni nalog 5", 4);
            InsertKos(5, dnid, DateTime.Now);
            InsertKos(15, dnid, DateTime.Now);
            InsertKos(25, dnid, DateTime.Now);
            InsertKos(35, dnid, DateTime.Now);
        }

        public void DropAndCreateNew()
        {
            conn.Open();
            cmd.CommandText = "drop database if exists ec38; create database ec38; use ec38; ";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE delovni_nalog (
                                   id              INT            NOT   NULL   AUTO_INCREMENT,
                                   st_kosov        INT,
                                   delovni_nalog   VARCHAR(30),
                                   PRIMARY   KEY   (id)
                                );



                                CREATE TABLE kosi (
                                   id              	INT            NOT   NULL   AUTO_INCREMENT,
                                   delovni_nalog_id INT,
                                   guid            	INT,
                                   cas_vnosa       	TIMESTAMP      NOT   NULL DEFAULT CURRENT_TIMESTAMP,
                                   PRIMARY   KEY   (id),
                                   FOREIGN KEY (delovni_nalog_id) 			REFERENCES delovni_nalog(id)
                                );

                                CREATE INDEX idx_guid ON kosi (guid); ";
            cmd.ExecuteNonQuery();

            conn.Close();
            Console.WriteLine("Table kosi created");
        }

        public int InsertDelovniNalog(string delovni_nalog, int st_kosov)
        {
            conn.Open();
            cmd.CommandText = String.Format("INSERT INTO delovni_nalog(delovni_nalog, st_kosov) " +
                                                "VALUES('{0}', {1}); select last_insert_id();",
                                                delovni_nalog, st_kosov);
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            conn.Close();

            return id;
        }

        public Boolean InsertKos(int guid, int delovni_nalog_id, DateTime cas_vnosa)
        {
            conn.Open();
            cmd.CommandText = String.Format("INSERT INTO kosi(guid, delovni_nalog_id, cas_vnosa) " +
                                                "VALUES({0}, '{1}', '{2}')",
                                                guid, delovni_nalog_id, cas_vnosa.ToString(time_format));
            int n_of_rows_affected = cmd.ExecuteNonQuery();

            conn.Close();
            if (n_of_rows_affected == 1)
            {
                return true;
            }
            return false;
        }

        public Kos GetPart(int guid)
        {
            string Query = "select * from kosi where guid=" + guid + ";";
            MySqlCommand cmd = new MySqlCommand(Query, conn);
            conn.Open();
            Kos result = null;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = new Kos
                    {
                        Id = Int32.Parse(reader["id"].ToString()),
                        Guid = Int32.Parse(reader["guid"].ToString()),
                        CasVnosa = DateTime.Parse(reader["cas_vnosa"].ToString()),
                    };
                }
            }

            conn.Close();
            return result;
        }

        public List<Kos> SelectAllParts()
        {
            string Query = "select * from kosi;";
            MySqlCommand cmd = new MySqlCommand(Query, conn);
            conn.Open();
            List<Kos> result = new List<Kos>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Kos kos = new Kos
                    {
                        Id = Int32.Parse(reader["id"].ToString()),
                        Guid = Int32.Parse(reader["guid"].ToString()),
                        CasVnosa = DateTime.Parse(reader["cas_vnosa"].ToString()),
                    };
                    result.Add(kos);
                }
            }

            conn.Close();
            return result;
        }
    }
}
