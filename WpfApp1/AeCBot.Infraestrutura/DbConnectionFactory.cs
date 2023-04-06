using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AeCBot.Infraestrutura
{
    public class DbConnectionFactory
    {
        private const string connectionString = "Server=np:\\\\.\\pipe\\LOCALDB#2B3F5747\\tsql\\query;Database=TesteAeC;Integrated Security=true;";


        public static IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
