using System;
using System.Linq;
using Baseline;
using BlueMilk;
using BlueMilk.Codegen;
using BlueMilk.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StructureMap.AspNetCore;

public class BenchmarkBase : IDisposable
{
    private IWebHost _blueMilkHost;
    private IWebHost _smHost;
    private IWebHost _aspnetHost;

    public BenchmarkBase()
    {
        var builder = new WebHostBuilder();
        builder
            .UseBlueMilk()
            .UseUrls("http://localhost:5002")
            .UseServer(new NulloServer())
            .UseStartup<Startup>();

        _blueMilkHost = builder.Start();


        var builder2 = new WebHostBuilder();
        builder2
            .UseUrls("http://localhost:5002")
            .ConfigureServices(services =>
            {
                services.AddMvc();
                services.AddLogging();
            })
            .UseServer(new NulloServer())
            .UseStartup<Startup2>()
            .UseStructureMap();

        _smHost = builder2.Start();


        var builder3 = new WebHostBuilder();
        builder3
            .UseUrls("http://localhost:5002")
            .ConfigureServices(services =>
            {
                services.AddMvc();
                services.AddLogging();
            })
            .UseServer(new NulloServer())
            .UseStartup<Startup3>();

        _aspnetHost = builder3.Start();

        Types = _blueMilkHost.Services.As<Container>().Model.AllInstances.Select(x => x.ServiceType)
            .Where(x => x.Assembly != typeof(Container).Assembly && !x.IsOpenGeneric())
            .Where(x => x != typeof(IServiceProviderFactory<ServiceRegistry>))
            .Where(x => !x.NameInCode().StartsWith("Func<"))
            .ToArray();

        

        Providers = new[] {_blueMilkHost.Services, _smHost.Services, _aspnetHost.Services};
    }

    public IServiceProvider BlueMilk => Providers[0];
    public IServiceProvider StructureMap => Providers[1];
    public IServiceProvider AspNetCore => Providers[2];
    public IServiceProvider[] Providers { get; set; }
    public Type[] Types { get; }

    public void Dispose()
    {
        _blueMilkHost?.Dispose();
        _smHost?.Dispose();
        _aspnetHost?.Dispose();
    }
}