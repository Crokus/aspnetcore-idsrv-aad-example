using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Wolnik.Client2.Services;

namespace Wolnik.Client2 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services.AddScoped<ISensorDataHttpClient, SensorDataHttpClient>();

            services.AddAuthentication(options => {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", options => options.AccessDeniedPath = "/Authorization/AccessDenied")
                .AddOpenIdConnect("oidc", options => {
                    options.Authority = "https://localhost:44392/";
                    options.RequireHttpsMetadata = true;
                    options.ClientId = "sensoradminclient";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("adminapi");
                    options.Scope.Add("division");
                    options.ResponseType = "code id_token";
                    options.SaveTokens = true;
                    options.ClientSecret = "secret2";
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
