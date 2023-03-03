using Aimtracker.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aimtracker.Repositories;
using Aimtracker.Infrastructure;
using System.Globalization;
using Microsoft.Extensions.Options;
using Aimtracker.Models;

namespace Aimtracker
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

            //string connection = Configuration["ConnectionStrings:Develop"];

            string connection = Configuration["ConnectionStrings:Default"];

            services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(connection,
                options => options.SetPostgresVersion(new Version(9, 5))));

            services.AddScoped<IBiathlonRepository, BiathlonRepository>();
            services.AddScoped<IWeatherRepository, WeatherRepository>();
            //services.AddScoped<IWeatherRepository, MockRepository>();
            services.AddScoped<IDbRepository, DbRepository>();

            services.AddScoped<IApiClient, ApiClient>();

            services.AddDbContext<LoginDbContext>(
                options =>options.UseNpgsql(connection,
                options => options.SetPostgresVersion(new Version(9, 5))));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<LoginDbContext>();

            services.AddControllersWithViews();            
           
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("se")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }


    }
}
