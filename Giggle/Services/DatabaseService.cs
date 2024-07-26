using Giggle.Configurations;
using MySql.Data.MySqlClient;
using System.Data;

namespace Giggle.Services
{
    public class DatabaseService
    {
        private readonly CustomConfigurationManager _configManager;

        public DatabaseService(CustomConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(GetConnectionString());
        }

        private string GetConnectionString()
        {
            return $"Server={_configManager.DbHost};Database={_configManager.DbName};User={_configManager.DbUsername};Password={_configManager.DbPassword};Charset={_configManager.DbCharset};";
        }
    }
}
