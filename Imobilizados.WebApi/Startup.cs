﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Http;

using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Imobilizados.WebApi
{
    public class Startup
    {
        private Container _container = new Container();
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InitializeMappingEntitiesAndDtos();
            services.AddMvc();
            IntegrateSimpleInjector(services);   
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
            _container.RegisterMongoDbRepositoriesAndMap(Configuration);
        }
    }
}
