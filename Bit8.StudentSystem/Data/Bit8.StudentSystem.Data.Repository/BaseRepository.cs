using System;

using Bit8.StudentSystem.Data.Interfaces;

namespace Bit8.StudentSystem.Data.Repository
{
    public abstract class BaseRepository
    {
        private readonly IApplicationDbContext context;

        public BaseRepository(IApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("context", "An instance of DbContext is required to use this repository.");
            }

            this.context = dbContext;
        }

        public IApplicationDbContext Context
        {
            get
            {
                return this.context;
            }
        }
    }
}
