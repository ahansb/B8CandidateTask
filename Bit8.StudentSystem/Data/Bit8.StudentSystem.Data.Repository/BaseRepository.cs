using System;
using System.IO;

using Bit8.StudentSystem.Data.Interfaces;

namespace Bit8.StudentSystem.Data.Repository
{
    public abstract class BaseRepository
    {
        private const string LogBasePath = @"..\..\";
        private readonly IApplicationDbContext context;
        protected readonly string disciplineTableName;
        protected readonly string semesterTableName;
        protected readonly string studentTableName;
        protected readonly string studentSemesterTableName;
        protected readonly string scoreTableName;

        public BaseRepository(IApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("context", "An instance of DbContext is required to use this repository.");
            }

            this.context = dbContext;

            this.disciplineTableName = $"{this.Context.GetDatabaseName()}.discipline";
            this.semesterTableName = $"{this.Context.GetDatabaseName()}.semester";
            this.studentTableName = $"{this.Context.GetDatabaseName()}.student";
            this.studentSemesterTableName = $"{this.Context.GetDatabaseName()}.studentsemester";
            this.scoreTableName = $"{this.Context.GetDatabaseName()}.score";
        }

        public IApplicationDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public void Log(string text)
        {
            var fullPath = $"{LogBasePath}\\Log-{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            using (var writer = new StreamWriter(fullPath, append: true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {text}");
            }
        }

        public string GetExceptionText(Exception ex)
        {
            var text = string.Empty;
            if (ex.InnerException == null)
            {
                text = ex.Message;
            }
            else
            {
                text = $"[ERROR] - {ex.Message} - Inner: {ex.InnerException.Message}";
            }

            return text;
        }
    }
}
