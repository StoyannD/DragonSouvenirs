namespace DragonSouvenirs.Web.Infrastructure
{
    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data;
    using DragonSouvenirs.Data.Seeding;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            // Seed data on application startup
            using var serviceScope = app.ApplicationServices.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

            return app;
        }

        public static IApplicationBuilder ApplyEnvironmentSettings(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseMigrationsEndPoint();
            }
            else
            {
                app
                    .UseExceptionHandler("/Home/Error")
                    .UseHsts();
            }

            return app;
        }

        public static IApplicationBuilder ApplyCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        GlobalConstants.Routes.CategoriesRoute,
                        "/{name:minlength(2)}",
                        new { controller = "Categories", action = "ByName" });
                    endpoints.MapControllerRoute(
                        "areaRoute",
                        "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });

            return app;
        }
    }
}
