using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SEE.PostfixAdmin.BackEnd.Common.Const;
using SEE.PostfixAdmin.BackEnd.Infrastructure;
using SEE.PostfixAdmin.BackEnd.BLL.Mailbox;
using SEE.PostfixAdmin.BackEnd.BLL.Domain;
using SEE.PostfixAdmin.BackEnd.BLL.Identity;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Domain;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration;
using SEE.PostfixAdmin.BackEnd.Common.Configuration;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias;
using SEE.PostfixAdmin.BackEnd.BLL.Alias;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;

namespace SEE.PostfixAdmin.FrontEnd
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Add configuration
            services.AddOptions();
            services.Configure<PasswordConfiguration>(Configuration.GetSection("PasswordConfiguration"));
            services.AddSingleton<IConfiguration>(Configuration);

            // Add application services
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DataContext")))
                ;

            services
                .AddTransient<IDomainService, DomainService>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IMailboxService, MailboxService>()
                .AddTransient<IAliasService, AliasService>()
                ;

            services
                .AddTransient<IMailboxRepository, MailboxRepository>()
                .AddTransient<IDomainRepository, DomainRepository>()
                .AddTransient<IConfigurationRepository, ConfigurationRepository>()
                .AddTransient<IAliasRepository, AliasRepository>()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = AppSettings.IdentityInstanceCookieName,
                LoginPath = new PathString("/Account/Login/"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
