using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Interfaces
{
    public interface IApplicationDbContext
    {
        void Initialize();
        void OpenConnection();
        void CloseConnection();
        MySqlDataReader ExecuteQuery(string statement);
        int ExecuteNonQuery(string statement, ICollection<MySqlParameter> parameters);
    }
}
