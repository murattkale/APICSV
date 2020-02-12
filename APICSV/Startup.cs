using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;

namespace APICSV
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new CsvMediaTypeFormatter());
            })
                .AddXmlSerializerFormatters();

            //services
            //   .AddMvc(option => option.EnableEndpointRouting = false)
            //   .AddNewtonsoftJson(opt =>
            //   {
            //       opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //       opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //       opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //   }
            //   );


            services.AddScoped(typeof(ICsvParserService), typeof(CsvParserService));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
