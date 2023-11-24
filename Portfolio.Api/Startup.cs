using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Portfolio.Crosscutting.DI;
using Portfolio.Crosscutting.Mapping;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Filters;
using Portfolio.Infrastructure.Helpers;
using Portfolio.Infrastructure.Security;
using System.Globalization;
using System.Text;

namespace Portfolio.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowFrontEnd", options => options.WithOrigins(appSettings.WebUrl + "/"));
            });

            services.AddDistributedMemoryCache();

            services.AddDependencyInjection();
            services.AddAutoMapper();
            services.AddMvcCore(options => { options.Filters.Add(typeof(CodigoUsuarioActionFilter)); }).AddApiExplorer();
            services.AddSwagger("Portfolio", "v1");
            services.AddIdentityServer()
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = b =>
                            b.UseSqlServer(Configuration.GetConnectionString("Portfolio")); //todo: set connectionString
                    });
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            var key = Encoding.ASCII.GetBytes(appSettings.SecretToken);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = appSettings.AudienceApi,
                        ValidIssuer = appSettings.Issuer
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseExceptionHandler(builder =>
            {
                builder.Run(
                    async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature.Error;
                        var result = JsonConvert.SerializeObject(new { error = exception.Message });
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(result);
                    });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio V1");
            });
            app.UseMiddleware<TokenManagerMiddleware>();

            app.UseCors(options => options.WithOrigins(Configuration["AppSettings:WebUrl"]).AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}