using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExceptionHandlingDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExceptionHandlingDemo
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
            services.AddControllersWithViews();
            services.AddControllersWithViews(config => config.Filters.Add(typeof(CustomExceptionFilter)));
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

               // app.UseExceptionHandler("/Home/Error");
               app.UseExceptionHandler(errorApp =>
               {
                   errorApp.Run(async context =>
                   {
                       context.Response.StatusCode = 500;
                       context.Response.ContentType = "text/html";

                       await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                       await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                       var exceptionHandlerPathFeature =
                           context.Features.Get<IExceptionHandlerPathFeature>();

                       if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                       {
                           await context.Response.WriteAsync(
                                                     "File error thrown!<br><br>\r\n");
                       }


                       if (exceptionHandlerPathFeature?.Error is DivideByZeroException)
                       {
                           await context.Response.WriteAsync(
                                                     "No.cant be divided by 0 !<br><br>\r\n");
                       }

                       await context.Response.WriteAsync(
                                                     "<a href=\"/\">Home</a><br>\r\n");
                       await context.Response.WriteAsync("</body></html>\r\n");
                       await context.Response.WriteAsync(new string(' ', 512));
                   });
               });

            }
            app.UseStatusCodePagesWithRedirects("/Home/MyStatusCode?code={0}");

            // app.UseStatusCodePages();
           // app.UseStatusCodePages( "text/plain", "Status code page, status code: {0}");

            app.UseStaticFiles();

            app.UseRouting();




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
