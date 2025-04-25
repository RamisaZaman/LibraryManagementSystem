using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;

namespace LibraryDatabase
{
    public class databaseHelper
    {
        private static NpgsqlConnection connectToDB()
        {
            string connStr = "Host=localhost;" +
                             "Port=5432;" +
                             "Database=restored_db;" +
                             "Username=postgres;" +
                             "Password=ramisazaman;";  // Replace with your local postgres password

            return new NpgsqlConnection(connStr);
        }

        public static DataTable dbRead(string query)
        {
            using (NpgsqlConnection connection = connectToDB()) {
                connection.Open();

                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                NpgsqlDataAdapter nda = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                nda.Fill(dt);
                return dt;
            }
        }

        public static void dbModify(string query) {                 // For modifying commands that do not return data like select
            using (NpgsqlConnection connection = connectToDB())
            {
                connection.Open();

                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }
    }
}