using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Bit8.StudentSystem.Data.Repository.Tests
{
    public abstract class BaseRepositoryTest
    {
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
