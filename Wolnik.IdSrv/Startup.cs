using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Wolnik.IdSrv {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services
            .AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddTestUsers(Config.GetUsers())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryClients(Config.GetClients());

            services.AddAuthentication()
            .AddOpenIdConnect("AAD", "Azure Active Directory", options => {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.Authority = "https://login.microsoftonline.com/common";
                options.ClientId = "d0abb606-ebd5-4939-903f-f2c702ff40b0";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false
                };
                options.GetClaimsFromUserInfoEndpoint = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();
            loggerFactory.AddFile("Logs/{Date}-idsrv-log.txt");
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
