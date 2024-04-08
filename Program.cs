using InjectionMoldingMachineDataAdapter.Application.Services;
using InjectionMoldingMachineDataAdapter.Application.Workers;
using InjectionMoldingMachineDataAdapter.Infrastructure.Communication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        var config = builder.Configuration;

        services.AddHostedService<UpdateWorkOrderWorker>();

        services.Configure<MqttOptions>(config.GetSection("MqttOptions"));
        services.AddSingleton<ManagedMqttClient>();
        services.AddSingleton(h => new MomApiCaller(
            h.GetRequiredService<RestClient>(),
            config.GetSection("MomApiUrl").Value!));
        services.AddSingleton<InjectionMoldingMachineObserver>();
        services.AddSingleton<RestClient>();
        services.AddSingleton(new HttpClient());
        services.AddSingleton<ManagedMqttClient>();
    })
    .Build();

await host.RunAsync();