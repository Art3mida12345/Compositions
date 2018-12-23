using System.Data.Entity;

namespace Installer
{
    class Program
    {
        static void Main()
        {
            Database.SetInitializer(new OracleInitializer());
        }
    }
}
