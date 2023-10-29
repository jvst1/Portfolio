using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Services.Cad;
using Portfolio.Application.Services.Identity;
using Portfolio.Crosscutting.Mapping;
using Portfolio.Data;
using Portfolio.Data.Context.DbServices;
using Portfolio.Data.Repositories.Cad;
using Portfolio.Data.Security.Factory;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Repositories.Cad;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Helpers;
using Portfolio.Infrastructure.Security;

namespace Portfolio.Crosscutting.DI
{
    public static class ApiDiConfig
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            ConfigureHttpContexts(services);

            ConfigureScoped(services);

            ConfigureTransientBase(services);

            ConfigureApplicationServices(services);

            ConfigureDomainServices(services);

            ConfigureServices(services);

            ConfigureRepositories(services);

            ConfigureMiddlewares(services);
        }


        private static void ConfigureMiddlewares(IServiceCollection services)
        {
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ConfigurationDbContextFactory>();
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
			services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TokenManager>();
            services.AddTransient<PortfolioEfDbService>();
        }

        private static void ConfigureDomainServices(IServiceCollection services)
        {
            services.AddTransient<UsuarioDomainService>();
        }

        private static void ConfigureApplicationServices(IServiceCollection services)
        {
            services.AddTransient<UsuarioApplicationService>();
            services.AddTransient<ApiResourcesApplicationService>();
            services.AddTransient<ApiScopeApplicationService>();
            services.AddTransient<ApiResourceSecretsApplicationService>();
            services.AddTransient<ClientsApplicationService>();
            services.AddTransient<ClientScopesApplicationService>();
            services.AddTransient<ClientSecretsApplicationService>();
        }

        private static void ConfigureTransientBase(IServiceCollection services)
        {
            services.AddTransient(typeof(IMappingService<>), typeof(AutoMapperBase<>));
            services.AddTransient<IDbProvider, DbProvider>();
        }

        private static void ConfigureScoped(IServiceCollection services)
        {
            services.AddScoped<CodigoUsuarioHelper>();
        }

        private static void ConfigureHttpContexts(IServiceCollection services)
        {
            services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<JwtHandler>();
        }
    }
}
