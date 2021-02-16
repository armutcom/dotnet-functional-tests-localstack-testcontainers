using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Armut.Api.Core;
using Armut.Api.Core.Installers;
using Armut.Api.Core.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Armut.Api
{
    public class Startup
    {
        private IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                .AddFluentValidation(configuration =>
                {
                    ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
                    configuration.RegisterValidatorsFromAssemblyContaining<AddUserModel>();
                });

            services
                .AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Armut.Api", Version = "v1"}); })
                .AddAutoMapper(typeof(MappingProfile))
                .InstallServices(Configuration, WebHostEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Armut.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
