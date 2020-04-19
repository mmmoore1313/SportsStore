using System;using System.Collections.Generic;using System.Linq;using System.Threading.Tasks;using Microsoft.AspNetCore.Builder;using Microsoft.AspNetCore.Hosting;using Microsoft.AspNetCore.Http;using Microsoft.Extensions.DependencyInjection;using SportsStore.Models;using Microsoft.Extensions.Configuration;using Microsoft.EntityFrameworkCore;namespace SportsStore
{
    public class Startup
    {

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Fore more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportsStoreProducts:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(                    name: null,                    pattern: "{category}/Page{productPage:int}",                    defaults: new
                    {
                        controller = "Product",
                        action = "List"
                    });
                endpoints.MapControllerRoute(                    name: null,                    pattern: "Page{productPage:int}",                    defaults: new
                    {
                        controller = "Product",
                        action = "List",
                        productPage = 1
                    });

                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{category}",
                    defaults: new
                    {
                        controller = "Product",
                        action = "List",
                        productPage = 1
                    });

                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "",
                    defaults: new
                    {
                        controller = "Product",
                        action = "List",
                        productPage = 1
                    });

                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{controller}/{action}/{id?}");
            });

            SeedData.EnsurePopulated(app);
        }
    }}