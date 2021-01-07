using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace TrickX
{
    class MySql_CONN
    {

        public string CONNECTION_STRING =
            "Server=94.103.91.136;" +
            "User=guli;" +
            "database=trick_surf;" +
            "port=3306;" +
            "password=gg;";

        public MySqlConnection CONN;

        public void OpenConnection()
        {


            CONN = new MySqlConnection(CONNECTION_STRING);
            try
            {
                CONN.Open();
            }
            catch (Exception e)
            {
                OpenConnection();
            }
        }

        public void OpenConnection(MySqlConnectionStringBuilder str)
        {

            CONN = new MySqlConnection(str.ConnectionString);
            try
            {
                CONN.Open();
            }
            catch (Exception e)
            {
                OpenConnection();
            }
        }



        public void CloseConnection()
        {
            CONN.Close();
        }

        public void Insert(string cmdText)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdText, CONN);

                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
            }
            catch (Exception err)
            {

            }
        }

        public void Delete(string cmdText)
        {
            Insert(cmdText);
        }

        public void Update(string cmdText)
        {
            Insert(cmdText);
        }



        public List<String> Select(string query)
        {
            try
            {
                List<String> res = new List<String>();
                MySqlCommand q = new MySqlCommand(query, CONN);
                MySqlDataReader r = q.ExecuteReader();

                while (r.Read())
                {
                    for (int inc = 0; inc < r.FieldCount; inc++)
                    {
                        res.Add(r[inc].ToString());
                    }
                }
                r.Close();

                return res;
            }
            catch (Exception err)
            {
                return new List<String>();
            }
        }
    }
}
