using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace ErrorLogging
{
    public class ErrorLog
    { 
        public static void Log(Exception Exp = null, string ExtraInfo = null)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
             
            try 
            {

                conn = new SqlConnection(WebConfigurationManager.AppSettings["DNS"].ToString());
                cmd = conn.CreateCommand();
                cmd.CommandText = "ErrorLog_Insert";
                cmd.CommandType = CommandType.StoredProcedure;

                if (Exp != null)
                {
                    cmd.Parameters.AddWithValue("@ErrorMsg", Exp.Message);
                    cmd.Parameters.AddWithValue("@ErrorStackTrace", Exp.StackTrace);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ErrorMsg", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ErrorStackTrace", DBNull.Value);
                }

                if (ExtraInfo != null)
                    cmd.Parameters.AddWithValue("@ExtraInfo", ExtraInfo);
                else
                    cmd.Parameters.AddWithValue("@ExtraInfo", DBNull.Value);


                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();

                if (cmd != null)
                    cmd.Dispose();
            }
        }
    }
}
