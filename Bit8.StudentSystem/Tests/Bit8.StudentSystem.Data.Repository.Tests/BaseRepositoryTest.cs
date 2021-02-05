using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Bit8.StudentSystem.Data.Repository.Tests
{
    public abstract class BaseRepositoryTest
    {
        public IApplicationDbContext ApplicationDbContext { get; set; }

        public string DisciplineTableName { get { return $"{this.ApplicationDbContext.GetDatabaseName()}.discipline"; } }
        public string SemesterTableName { get { return $"{this.ApplicationDbContext.GetDatabaseName()}.semester"; } }
        public string StudentTableName { get { return $"{this.ApplicationDbContext.GetDatabaseName()}.student"; } }
        public string StudentSemesterTableName { get { return $"{this.ApplicationDbContext.GetDatabaseName()}.studentsemester"; } }
        public string ScoreTableName { get { return $"{this.ApplicationDbContext.GetDatabaseName()}.score"; } }


        public static IConfiguration Configuration
        {
            get
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
                return config;
            }
        }

    }
}
