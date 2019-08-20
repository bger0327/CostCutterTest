using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ui.Properties;

namespace ui
{
    public class Database
    {
        private readonly string _ConnectionString = Settings.Default.DbConnectionString;

        public MySqlConnection GetConnection => new MySqlConnection(_ConnectionString);
    }
}
