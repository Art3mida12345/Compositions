using System;
using System.Data.Entity;
using Oracle.ManagedDataAccess.Client;

namespace Installer
{
    public class OracleInitializer : CreateDatabaseIfNotExists<OracleDbContext>
    {
        protected override void Seed(OracleDbContext context)
        {
            OracleConnection conn = new OracleConnection("OracleDbContext");
            try
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "create user lyb identified by 123;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "grant connect, resource to lyb;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "grant create view to lyb;";
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong...");
            }
            finally
            {
                conn.Close();
            }


            base.Seed(context);
        }
    }
}
