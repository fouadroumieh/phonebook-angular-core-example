using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhoneBook.Web.Repository;
using PhoneBook.Web.Validation.Filters;
using Serilog;

namespace PhoneBook.Web
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                    .AddMvcOptions(options => options.Filters.Add(new ValidateInputFilter(Log.Logger)));

            var serviceProvider = ConfigureAutofac(services);

            return serviceProvider;
        }
        private void UseLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddSerilog(Log.Logger);
            loggerFactory.AddDebug();
        }
        protected virtual IServiceProvider ConfigureAutofac(IServiceCollection services)
        {
            var container = new ContainerBuilder();

            container.RegisterType<PhoneBookRepository>().As<IPhoneBookRepository>();

            container.RegisterInstance(Configuration);
            container.Populate(services);

            var builtContainer = container.Build();

            return new AutofacServiceProvider(builtContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            UseLogging(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });


        }
    }
}
