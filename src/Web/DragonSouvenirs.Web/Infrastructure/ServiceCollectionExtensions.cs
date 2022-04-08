namespace DragonSouvenirs.Web.Infrastructure
{
    using System;

    using CloudinaryDotNet;

    using DragonSouvenirs.Data;
    using DragonSouvenirs.Data.Common;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Repositories;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Services.Messaging;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName = "dbo";
                options.TableName = "CacheData";
            });

            return services;
        }

        public static IServiceCollection AddSessions(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                // JS does not see the cookie (XSS security)
                options.Cookie.HttpOnly = true;

                options.IdleTimeout = TimeSpan.FromDays(2);
            });

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureCookies(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            return services;
        }

        public static IServiceCollection AddControllersWithViewsAndRazor(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            return services;
        }

        public static IServiceCollection AddExternalAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Facebook and Google authentication
            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = configuration["Authentication:Facebook:AppId"];
                    options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });

            return services;
        }

        public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinary = new Cloudinary(
                new Account(
                    configuration["Cloudinary:CloudName"],
                    configuration["Cloudinary:ApiKey"],
                    configuration["Cloudinary:ApiSecret"]));
            services.AddSingleton(cloudinary);

            return services;
        }

        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender>(_ => new SendGridEmailSender(configuration["SendGrid:ApiKey"]));
            services.AddTransient<IEmailTemplatesSender, EmailTemplatesService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOfficeService, OfficeService>();
            services.AddTransient<ICommonFeaturesService, CommonFeaturesService>();

            return services;
        }
    }
}
