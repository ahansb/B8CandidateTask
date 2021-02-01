using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Interfaces;

namespace Bit8.StudentSystem.Data.Repository
{
    public abstract class BaseRepository : IDisposable
    {
        private readonly IApplicationDbContext context;

        public BaseRepository(IApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("context", "An instance of DbContext is required to use this repository.");
            }

            this.context = dbContext;
            this.Context.OpenConnection();
        }

        public IApplicationDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public void Dispose()
        {
            this.Context.CloseConnection();
        }
    }
}
