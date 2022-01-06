using EV5.Mvc;
using EV5.Mvc.Extensions;
using EV5.Mvc.MEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EV5.Examples
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
            //services.AddRazorPages();
            services.AddRazorPages()
                .AddViewOptions(o => o.ViewEngines.Insert(0, new EmbeddedViewEngine("eve-")));
            //this call sets up the default EV5 Services
            services.AddEV5DefaultServices();


            //This method will discover all exported IEmbeddedPlugins in the provided CompositionHostFactory
            //It will then use the information in these objects to set up the web components and.
            services.UseEmbeddedPlugins(new DirCompositionHostFactory(AppDomain.CurrentDomain.BaseDirectory, "EV5*.dll"));


            //Always call this last. The internal EV5 ServiceProvider will be registered at this point,
            //It will only be able to find services registered so far
            services.RegisterEV5ServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            env.ConfigureEmbeddedPlugins();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
