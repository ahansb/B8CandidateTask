﻿using System;
using System.Collections.Generic;
using System.IO;

using Bit8.StudentSystem.Data.Interfaces;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly string connectionString;
        private const string BasePath = @"..\..\Data\Bit8.StudentSystem.Data\SQLs\";

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
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
            var path = $"{BasePath}{sqlName}";

            if (File.Exists(path))
            {
                statement = File.ReadAllText(path);
            }

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
            var dbName = this.GetDatabaseName(this.connectionString);
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

        private string GetDatabaseName(string connectionString)
        {
            var databaseString = "Database=";
            var indexOfDbNameStart = connectionString.IndexOf(databaseString) + databaseString.Length;
            var indexOfDbNameEnd = connectionString.IndexOf(';', indexOfDbNameStart);
            return connectionString.Substring(indexOfDbNameStart, indexOfDbNameEnd - indexOfDbNameStart);
        }
    }
}
