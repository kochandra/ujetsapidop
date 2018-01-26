using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using uimgapi.Models;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;

namespace uimgapi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string Server = Environment.GetEnvironmentVariable("DB_Server");
            string UserID = Environment.GetEnvironmentVariable("DB_User");
            string Password = Environment.GetEnvironmentVariable("DB_Password");
            string Database = Environment.GetEnvironmentVariable("DB_Database");
            string connectionString = string.Format("Server = {0}; User Id = {1}; Password = {2}; Database = {3}", Server, UserID,Password,Database);
            Console.WriteLine(connectionString);
            //string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //Add Identity
            services.AddDbContext<s3uploadtestContext>(options =>
            options.UseMySql(connectionString));

            services.AddCors();
            services.AddMvc();
            //services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            //services.AddAWSService<IAmazonS3>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors( p=> p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();
        }
    }
}
