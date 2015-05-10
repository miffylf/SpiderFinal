using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace LuceneSearch
{
    class SqlHelper
    {
        public static readonly string connstr =
            ConfigurationManager.ConnectionStrings["dbconnStr"].ConnectionString;

        public static int ExecuteNonQuery(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static SqlDataReader ExecuteDataReader(string cmdText,
            params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static int ExecuteStoredProcedure(string procName,
            params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = procName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
