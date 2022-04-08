namespace DragonSouvenirs.Web
{
    using System.Reflection;

    using DragonSouvenirs.Data;

    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.Infrastructure;
    using DragonSouvenirs.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(this.configuration.GetDefaultConnectionString()))
                .AddDistributedCache(this.configuration, this.configuration.GetDefaultConnectionString())
                .AddSessions()
                .AddIdentity()
                .ConfigureCookies()
                .AddControllersWithViewsAndRazor()
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddSingleton(this.configuration)
                .AddDataRepositories()
                .AddApplicationServices(this.configuration)
                .AddExternalAuthentication(this.configuration)
                .AddCloudinary(this.configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            app
                .ApplyMigrations()
                .ApplyEnvironmentSettings(env)
                .UseStatusCodePagesWithReExecute("/Error/PageNotFound")
                .UseSession()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .ApplyCustomEndpoints();
        }
    }
}
