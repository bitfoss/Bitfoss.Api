using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bitfoss.Api.Services;
using Bitfoss.Api.Services.Dummy;
using Bitfoss.Api.Models.Options;
using Bitfoss.Api.Data.Repository;
using Bitfoss.Api.Data.Repository.MySql;

namespace Bitfoss.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Options
            services.Configure<ApiClientsOptions>(_configuration);
            services.Configure<SmtpServiceOptions>(_configuration.GetSection(nameof(SmtpServiceOptions)));
            services.Configure<MySqlRepositoryOptions>(_configuration.GetSection(nameof(MySqlRepositoryOptions)));

            // Add services
            services.AddTransient<IApiClientService, ApiClientService>();
            services.AddMySqlRepository();

            // Add environment-specific services
            if (_environment.IsDevelopment())
            {
                services.AddTransient<ISmtpService, DummySmtpService>();
            }
            else
            {
                services.AddTransient<ISmtpService, SmtpService>();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
