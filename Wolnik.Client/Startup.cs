using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Wolnik.Client.Services;

namespace Wolnik.Client {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services.AddScoped<ISensorDataHttpClient, SensorDataHttpClient>();

            // clear the MS remapping of claim types
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options => {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", options => options.AccessDeniedPath = "/Authorization/AccessDenied")
            .AddOpenIdConnect("oidc", options => {
                options.Authority = "http://localhost:11219/";
                options.RequireHttpsMetadata = false;
                options.ClientId = "sensorclient";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("sensorsapi");
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.ClientSecret = "secret";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = "name"
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sensor}/{action=Index}/{id?}");
            });
        }
    }
}
