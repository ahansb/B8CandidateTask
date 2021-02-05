using System;
using System.Collections.Generic;
using System.IO;

using Bit8.StudentSystem.Data.Interfaces;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly string connectionString;
        private readonly string basePath;

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
            basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\Data\Bit8.StudentSystem.Data\SQLs\"));
        }

        public MySqlConnection Connection { get { return new MySqlConnection(this.connectionString); } }

        public void Initialize()
        {
            this.DropDbIfExistsAndRecreate();
            this.CreateTables();
            this.SeedData();
        }

        private void SeedData()
        {
            this.ExecuteSql("SeedTables.sql");
        }

        private void CreateTables()
        {
            this.ExecuteSql("CreateTables.sql");
        }

        private void ExecuteSql(string sqlName)
        {
            var statement = string.Empty;
            var path = $"{basePath}{sqlName}";

            if (File.Exists(path))
            {
                statement = File.ReadAllText(path);
            }

            statement = statement.Replace("`bit8studentsystem`", $"`{this.GetDatabaseName()}`");

            using (var connection = this.Connection)
            {
                connection.Open();
                var command = new MySqlCommand(statement, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DropDbIfExistsAndRecreate()
        {
            var dbName = this.GetDatabaseName();
            var masterConnectionString = this.connectionString.Replace($"Database={dbName};", string.Empty);

            using (var connection = new MySqlConnection(masterConnectionString))
            {
                connection.Open();

                var statement = $"DROP SCHEMA IF EXISTS {dbName};";
                var command = new MySqlCommand(statement, connection);
                command.ExecuteNonQuery();

                statement = $"CREATE DATABASE IF NOT EXISTS {dbName};";
                command = new MySqlCommand(statement, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public string GetDatabaseName()
        {
            var databaseString = "Database=";
            var indexOfDbNameStart = this.connectionString.IndexOf(databaseString) + databaseString.Length;
            var indexOfDbNameEnd = this.connectionString.IndexOf(';', indexOfDbNameStart);
            return connectionString.Substring(indexOfDbNameStart, indexOfDbNameEnd - indexOfDbNameStart);
        }
    }
}
