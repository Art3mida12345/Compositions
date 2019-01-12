using System;

namespace WorkOfFiction.Constants
{
    public static class GlobalConstants
    {
        #region Connection
        public static readonly string Connection =
            "Data Source=" +
            $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.MachineName})" +
            "(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));" +
            "User Id=LYB;Password=123;";
        #endregion
    }
}