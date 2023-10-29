using Portfolio.Api;
using Portfolio.Crosscutting.DI;
using Portfolio.Infrastructure.Helpers;

var builder = Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                  .ConfigureAppConfiguration((hostContext, config) =>
                  {
                      config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                  })
                  .ConfigureServices((hostContext, services) =>
                  {
                      services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                  
                      ApiDiConfig.AddDependencyInjection(services);
                  });
