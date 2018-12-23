using System.Data.Entity;

namespace Installer
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext() : base("OracleDbContext")
        {
        }


    }
}
