using Gls_Etykiety.Configuration;
using Gls_Etykiety.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("local.settings.json")
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {

        builder.UseMiddleware<ExceptionHandlingMiddleware>();

        builder.Services.AddDbContextFactory<LabelDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Db")));
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();



        services.AddDbContext<LabelDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Db")));

    })
    .Build();

host.Run();
