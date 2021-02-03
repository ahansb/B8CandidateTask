using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bit8.StudentSystem.Data;
using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Services.Data;
using Bit8.StudentSystem.Services.Data.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bit8.StudentSystem.Web.Api
{
    public class Startup
    {
        private const string ApplicationDbContextName = "ApplicationDbContext";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IApplicationDbContext>(new ApplicationDbContext(this.Configuration.GetConnectionString(ApplicationDbContextName)));

            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<IStudentService, StudentService>();

            services.AddCors();
            services.AddControllers();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationDbContext dbContext)
        {
            dbContext.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
