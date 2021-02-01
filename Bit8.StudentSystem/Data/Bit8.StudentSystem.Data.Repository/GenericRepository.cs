using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository.Interfaces;

namespace Bit8.StudentSystem.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IApplicationDbContext context;
        private readonly string tableName;

        public GenericRepository(IApplicationDbContext dbContext, string tableName)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("context", "An instance of DbContext is required to use this repository.");
            }

            this.context = dbContext;
            this.tableName = tableName;
            this.Context.OpenConnection();
        }

        public IApplicationDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public ICollection<T> All()
        {
            var statement = $"SELECT * FROM {this.tableName};";
            var reader = this.Context.ExecuteQuery(statement);
            ICollection<T> result = new List<T>();

            while (reader.Read())
            {
                Type myType = typeof(T);
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                var createdGenericObject = (T)Activator.CreateInstance(myType);

                foreach (PropertyInfo property in props)
                {
                    property.SetValue(createdGenericObject, reader[property.Name]);
                }

                result.Add(createdGenericObject);
            }

            return result;
        }

        public void Dispose()
        {
            this.Context.CloseConnection();
        }
    }
}
