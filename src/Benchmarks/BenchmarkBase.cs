using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Baseline;
using BlueMilk;
using BlueMilk.Codegen;
using BlueMilk.IoC.Instances;
using BlueMilk.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StructureMap.AspNetCore;

public class BenchmarkBase : IDisposable
{
    protected IWebHost _blueMilkHost;


    public BenchmarkBase()
    {
        var builder = new WebHostBuilder();
        builder
            .UseBlueMilk()
            .ConfigureServices(services =>
            {
                var registry = new ServiceRegistry();
                configure(registry);

                services.AddRange(registry);
            })
            .UseUrls("http://localhost:5002")
            .UseServer(new NulloServer())
            .UseStartup<Startup>();

        _blueMilkHost = builder.Start();

        Instances = _blueMilkHost.Services.As<Container>().Model.AllInstances
            .Where(x => x.ServiceType.Assembly != typeof(Container).Assembly && !x.ServiceType.IsOpenGeneric())
            .Where(x => x.ServiceType != typeof(IServiceProviderFactory<ServiceRegistry>))
            .ToArray();


        Singletons = Instances.Where(x => x.Lifetime == ServiceLifetime.Singleton).Select(x => x.ServiceType)
            .Distinct().ToArray();

        Scoped = Instances.Where(x => x.Lifetime == ServiceLifetime.Scoped).Select(x => x.ServiceType)
            .Distinct().ToArray();

        Transients = Instances.Where(x => x.Lifetime == ServiceLifetime.Transient).Select(x => x.ServiceType)
            .Distinct().ToArray();

        Objects = Instances.OfType<ObjectInstance>().Select(x => x.ServiceType).Distinct().ToArray();

        Lambdas = Instances.OfType<LambdaInstance>().Select(x => x.ServiceType).Distinct().ToArray();

        Internals = Instances.Where(x => x.ImplementationType.IsNotPublic).Select(x => x.ServiceType).Distinct();
    }

    public IEnumerable<Type> Internals { get; set; }


    public Type[] Lambdas { get; set; }

    public Type[] Objects { get; set; }

    public Type[] Transients { get; set; }

    public Type[] Scoped { get; set; }

    public Type[] Singletons { get; set; }

    public Instance[] Instances { get; }

    protected virtual void configure(ServiceRegistry services)
    {
        
    }

    public IServiceProvider BlueMilk => _blueMilkHost.Services;

    public Type[] Types { get; }

    public virtual void Dispose()
    {
        _blueMilkHost?.Dispose();

    }


}

