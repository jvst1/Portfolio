using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Portfolio.Crosscutting.Mapping
{
    public static class ApiAutoMapperConfig
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(AutoMapperConfig.GetAllMappings());
            services.AddSingleton(config.CreateMapper());
        }
    }
}