using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imobilizados.API.Utils;
using Imobilizados.Application;
using Imobilizados.Application.Interfaces;
using Imobilizados.Domain.Repository;
using Imobilizados.Infrastructure;
using Imobilizados.Infrastructure.MemoryDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Imobilizados.Infrastructure.Logging;
using Imobilizados.Infrastructure.RabbitMQ;

namespace Imobilizados.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHardwareService, HardwareService>();
            services.AddScoped<IHardwareRepository, HardwareRepository>();
            services.AddSingleton<IMongoClient>((provider) => new MongoClient(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc( option => 
                {
                    option.Filters.Add<ActionLogger>();
                })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            AutoMapperConfiguration.Configure();
            MongoDbConfiguration.Map();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddRabbitMQ(Configuration, eventId: 2000);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                Console.WriteLine($"Is Production: {env.IsProduction()}");
                app.UseDeveloperExceptionPage();
            }

            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
