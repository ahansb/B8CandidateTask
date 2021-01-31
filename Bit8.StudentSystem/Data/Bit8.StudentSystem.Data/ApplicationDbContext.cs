using System;

using Bit8.StudentSystem.Data.Interfaces;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly string connectionString;

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Initialize()
        {
            var connection = new MySqlConnection(this.connectionString);
            connection.Open();

            var stm = "SELECT * FROM world.country LIMIT 1";
            var cmd = new MySqlCommand(stm, connection);

            var version = cmd.ExecuteScalar().ToString();
        }
    }
}
