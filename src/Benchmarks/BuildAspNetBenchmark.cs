using System;
using System.Linq;
using Baseline;
using BenchmarkDotNet.Attributes;
using BlueMilk;
using BlueMilk.Codegen;
using BlueMilk.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StructureMap.AspNetCore;

public class BuildAspNetBenchmark : BenchmarkBase
{
    public BuildAspNetBenchmark() : base()
    {
    }


    /*
        [ParamsSource(nameof(Providers))]
        public IServiceProvider Provider { get; set; }
        
        [ParamsSource(nameof(Types))]
        public Type CurrentType { get; set; }
*/


    /*
        [Benchmark]
        public void Resolve()
        {
            var value = Provider.GetService(CurrentType);
        }

        
        [Benchmark]
        public void Everything()
        {
            foreach (var type in Types)
            {
                var value = Provider.GetService(type);
            }
        }
        */

    /*
        [Benchmark]
        public void StructureMap()
        {
            var container = _smHost.Services.GetService<StructureMap.IContainer>();
            foreach (var type in Types)
            {
                var value = container.GetInstance(type);
            }
        }
        */

/*
        [Benchmark]
        public void BlueMilk()
        {
            var container = _blueMilkHost.Services.As<BlueMilk.Container>();
            foreach (var type in Types)
            {
                var value = container.GetInstance(type);
            }
        }
        */

    /*
        [Benchmark]
        public void AspNet()
        {
            foreach (var type in Types)
            {
                var value = _aspnetHost.Services.GetService(type);
            }
        }
        */


    [Benchmark]
    public void Type_1_BlueMilk()
    {
        var value = Providers[0].GetService(typeof(IHostingEnvironment));
    }

    [Benchmark]
    public void Type_1_StructureMap()
    {
        var value = Providers[1].GetService(typeof(IHostingEnvironment));
    }

    [Benchmark]
    public void Type_1_AspNetCore()
    {
        var value = Providers[2].GetService(typeof(IHostingEnvironment));
    }

/*
        [Benchmark]
        public void Type_2_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(WebHostBuilderContext));
        }

        [Benchmark]
        public void Type_2_StructureMap()
        {
            var value = Providers[1].GetService(typeof(WebHostBuilderContext));
        }

        [Benchmark]
        public void Type_2_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(WebHostBuilderContext));
        }


        [Benchmark]
        public void Type_3_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfiguration));
        }

        [Benchmark]
        public void Type_3_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfiguration));
        }

        [Benchmark]
        public void Type_3_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfiguration));
        }


        [Benchmark]
        public void Type_4_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationBuilderFactory));
        }

        [Benchmark]
        public void Type_4_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationBuilderFactory));
        }

        [Benchmark]
        public void Type_4_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationBuilderFactory));
        }


        [Benchmark]
        public void Type_5_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IHttpContextFactory));
        }

        [Benchmark]
        public void Type_5_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IHttpContextFactory));
        }

        [Benchmark]
        public void Type_5_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IHttpContextFactory));
        }


        [Benchmark]
        public void Type_6_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IMiddlewareFactory));
        }

        [Benchmark]
        public void Type_6_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IMiddlewareFactory));
        }

        [Benchmark]
        public void Type_6_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IMiddlewareFactory));
        }


        [Benchmark]
        public void Type_7_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILoggerFactory));
        }

        [Benchmark]
        public void Type_7_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILoggerFactory));
        }

        [Benchmark]
        public void Type_7_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILoggerFactory));
        }


        [Benchmark]
        public void Type_8_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_8_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_8_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<LoggerFilterOptions>));
        }


        [Benchmark]
        public void Type_9_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IStartupFilter));
        }

        [Benchmark]
        public void Type_9_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IStartupFilter));
        }

        [Benchmark]
        public void Type_9_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IStartupFilter));
        }


        [Benchmark]
        public void Type_10_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IStartupFilter));
        }

        [Benchmark]
        public void Type_10_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IStartupFilter));
        }

        [Benchmark]
        public void Type_10_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IStartupFilter));
        }


        [Benchmark]
        public void Type_11_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IServiceProviderFactory<IServiceCollection>));
        }

        [Benchmark]
        public void Type_11_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IServiceProviderFactory<IServiceCollection>));
        }

        [Benchmark]
        public void Type_11_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IServiceProviderFactory<IServiceCollection>));
        }


        [Benchmark]
        public void Type_12_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IServiceProviderFactory<IServiceCollection>));
        }

        [Benchmark]
        public void Type_12_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IServiceProviderFactory<IServiceCollection>));
        }

        [Benchmark]
        public void Type_12_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IServiceProviderFactory<IServiceCollection>));
        }


        [Benchmark]
        public void Type_13_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ObjectPoolProvider));
        }

        [Benchmark]
        public void Type_13_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ObjectPoolProvider));
        }

        [Benchmark]
        public void Type_13_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ObjectPoolProvider));
        }


        [Benchmark]
        public void Type_14_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IServer));
        }

        [Benchmark]
        public void Type_14_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IServer));
        }

        [Benchmark]
        public void Type_14_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IServer));
        }


        [Benchmark]
        public void Type_15_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IStartup));
        }

        [Benchmark]
        public void Type_15_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IStartup));
        }

        [Benchmark]
        public void Type_15_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IStartup));
        }


        [Benchmark]
        public void Type_16_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(DiagnosticListener));
        }

        [Benchmark]
        public void Type_16_StructureMap()
        {
            var value = Providers[1].GetService(typeof(DiagnosticListener));
        }

        [Benchmark]
        public void Type_16_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(DiagnosticListener));
        }


        [Benchmark]
        public void Type_17_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(DiagnosticSource));
        }

        [Benchmark]
        public void Type_17_StructureMap()
        {
            var value = Providers[1].GetService(typeof(DiagnosticSource));
        }

        [Benchmark]
        public void Type_17_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(DiagnosticSource));
        }


        [Benchmark]
        public void Type_18_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationLifetime));
        }

        [Benchmark]
        public void Type_18_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationLifetime));
        }

        [Benchmark]
        public void Type_18_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationLifetime));
        }


        [Benchmark]
        public void Type_19_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(HostedServiceExecutor));
        }

        [Benchmark]
        public void Type_19_StructureMap()
        {
            var value = Providers[1].GetService(typeof(HostedServiceExecutor));
        }

        [Benchmark]
        public void Type_19_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(HostedServiceExecutor));
        }


        [Benchmark]
        public void Type_20_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ApplicationPartManager));
        }

        [Benchmark]
        public void Type_20_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ApplicationPartManager));
        }

        [Benchmark]
        public void Type_20_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ApplicationPartManager));
        }


        [Benchmark]
        public void Type_21_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IInlineConstraintResolver));
        }

        [Benchmark]
        public void Type_21_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IInlineConstraintResolver));
        }

        [Benchmark]
        public void Type_21_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IInlineConstraintResolver));
        }


        [Benchmark]
        public void Type_22_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(UrlEncoder));
        }

        [Benchmark]
        public void Type_22_StructureMap()
        {
            var value = Providers[1].GetService(typeof(UrlEncoder));
        }

        [Benchmark]
        public void Type_22_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(UrlEncoder));
        }


        [Benchmark]
        public void Type_23_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ObjectPool<UriBuildingContext>));
        }

        [Benchmark]
        public void Type_23_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ObjectPool<UriBuildingContext>));
        }

        [Benchmark]
        public void Type_23_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ObjectPool<UriBuildingContext>));
        }


        [Benchmark]
        public void Type_24_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(TreeRouteBuilder));
        }

        [Benchmark]
        public void Type_24_StructureMap()
        {
            var value = Providers[1].GetService(typeof(TreeRouteBuilder));
        }

        [Benchmark]
        public void Type_24_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(TreeRouteBuilder));
        }


        [Benchmark]
        public void Type_25_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RoutingMarkerService));
        }

        [Benchmark]
        public void Type_25_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RoutingMarkerService));
        }

        [Benchmark]
        public void Type_25_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RoutingMarkerService));
        }


        [Benchmark]
        public void Type_26_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_26_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_26_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<MvcOptions>));
        }


        [Benchmark]
        public void Type_27_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_27_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_27_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<MvcOptions>));
        }


        [Benchmark]
        public void Type_28_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_28_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_28_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<MvcOptions>));
        }


        [Benchmark]
        public void Type_29_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_29_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_29_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<MvcOptions>));
        }


        [Benchmark]
        public void Type_30_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<RouteOptions>));
        }

        [Benchmark]
        public void Type_30_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<RouteOptions>));
        }

        [Benchmark]
        public void Type_30_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<RouteOptions>));
        }


        [Benchmark]
        public void Type_31_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_31_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_31_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationModelProvider));
        }


        [Benchmark]
        public void Type_32_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_32_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_32_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationModelProvider));
        }


        [Benchmark]
        public void Type_33_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_33_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_33_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationModelProvider));
        }


        [Benchmark]
        public void Type_34_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_34_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationModelProvider));
        }

        [Benchmark]
        public void Type_34_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationModelProvider));
        }


        [Benchmark]
        public void Type_35_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionDescriptorProvider));
        }

        [Benchmark]
        public void Type_35_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionDescriptorProvider));
        }

        [Benchmark]
        public void Type_35_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionDescriptorProvider));
        }


        [Benchmark]
        public void Type_36_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionDescriptorProvider));
        }

        [Benchmark]
        public void Type_36_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionDescriptorProvider));
        }

        [Benchmark]
        public void Type_36_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionDescriptorProvider));
        }


        [Benchmark]
        public void Type_37_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionDescriptorCollectionProvider));
        }

        [Benchmark]
        public void Type_37_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionDescriptorCollectionProvider));
        }

        [Benchmark]
        public void Type_37_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionDescriptorCollectionProvider));
        }


        [Benchmark]
        public void Type_38_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionSelector));
        }

        [Benchmark]
        public void Type_38_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionSelector));
        }

        [Benchmark]
        public void Type_38_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionSelector));
        }


        [Benchmark]
        public void Type_39_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ActionConstraintCache));
        }

        [Benchmark]
        public void Type_39_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ActionConstraintCache));
        }

        [Benchmark]
        public void Type_39_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ActionConstraintCache));
        }


        [Benchmark]
        public void Type_40_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionConstraintProvider));
        }

        [Benchmark]
        public void Type_40_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionConstraintProvider));
        }

        [Benchmark]
        public void Type_40_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionConstraintProvider));
        }


        [Benchmark]
        public void Type_41_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IControllerFactory));
        }

        [Benchmark]
        public void Type_41_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IControllerFactory));
        }

        [Benchmark]
        public void Type_41_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IControllerFactory));
        }


        [Benchmark]
        public void Type_42_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IControllerActivator));
        }

        [Benchmark]
        public void Type_42_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IControllerActivator));
        }

        [Benchmark]
        public void Type_42_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IControllerActivator));
        }


        [Benchmark]
        public void Type_43_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IControllerFactoryProvider));
        }

        [Benchmark]
        public void Type_43_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IControllerFactoryProvider));
        }

        [Benchmark]
        public void Type_43_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IControllerFactoryProvider));
        }


        [Benchmark]
        public void Type_44_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IControllerActivatorProvider));
        }

        [Benchmark]
        public void Type_44_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IControllerActivatorProvider));
        }

        [Benchmark]
        public void Type_44_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IControllerActivatorProvider));
        }


        [Benchmark]
        public void Type_45_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IControllerPropertyActivator));
        }

        [Benchmark]
        public void Type_45_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IControllerPropertyActivator));
        }

        [Benchmark]
        public void Type_45_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IControllerPropertyActivator));
        }


        [Benchmark]
        public void Type_46_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IControllerPropertyActivator));
        }

        [Benchmark]
        public void Type_46_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IControllerPropertyActivator));
        }

        [Benchmark]
        public void Type_46_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IControllerPropertyActivator));
        }


        [Benchmark]
        public void Type_47_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionInvokerFactory));
        }

        [Benchmark]
        public void Type_47_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionInvokerFactory));
        }

        [Benchmark]
        public void Type_47_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionInvokerFactory));
        }


        [Benchmark]
        public void Type_48_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionInvokerProvider));
        }

        [Benchmark]
        public void Type_48_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionInvokerProvider));
        }

        [Benchmark]
        public void Type_48_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionInvokerProvider));
        }


        [Benchmark]
        public void Type_49_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionInvokerProvider));
        }

        [Benchmark]
        public void Type_49_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionInvokerProvider));
        }

        [Benchmark]
        public void Type_49_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionInvokerProvider));
        }


        [Benchmark]
        public void Type_50_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ControllerActionInvokerCache));
        }

        [Benchmark]
        public void Type_50_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ControllerActionInvokerCache));
        }

        [Benchmark]
        public void Type_50_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ControllerActionInvokerCache));
        }


        [Benchmark]
        public void Type_51_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IFilterProvider));
        }

        [Benchmark]
        public void Type_51_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IFilterProvider));
        }

        [Benchmark]
        public void Type_51_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IFilterProvider));
        }


        [Benchmark]
        public void Type_52_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RequestSizeLimitResourceFilter));
        }

        [Benchmark]
        public void Type_52_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RequestSizeLimitResourceFilter));
        }

        [Benchmark]
        public void Type_52_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RequestSizeLimitResourceFilter));
        }


        [Benchmark]
        public void Type_53_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(DisableRequestSizeLimitResourceFilter));
        }

        [Benchmark]
        public void Type_53_StructureMap()
        {
            var value = Providers[1].GetService(typeof(DisableRequestSizeLimitResourceFilter));
        }

        [Benchmark]
        public void Type_53_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(DisableRequestSizeLimitResourceFilter));
        }


        [Benchmark]
        public void Type_54_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IModelMetadataProvider));
        }

        [Benchmark]
        public void Type_54_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IModelMetadataProvider));
        }

        [Benchmark]
        public void Type_54_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IModelMetadataProvider));
        }


        [Benchmark]
        public void Type_55_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ICompositeMetadataDetailsProvider));
        }

        [Benchmark]
        public void Type_55_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ICompositeMetadataDetailsProvider));
        }

        [Benchmark]
        public void Type_55_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ICompositeMetadataDetailsProvider));
        }


        [Benchmark]
        public void Type_56_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IModelBinderFactory));
        }

        [Benchmark]
        public void Type_56_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IModelBinderFactory));
        }

        [Benchmark]
        public void Type_56_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IModelBinderFactory));
        }


        [Benchmark]
        public void Type_57_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IObjectModelValidator));
        }

        [Benchmark]
        public void Type_57_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IObjectModelValidator));
        }

        [Benchmark]
        public void Type_57_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IObjectModelValidator));
        }


        [Benchmark]
        public void Type_58_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ClientValidatorCache));
        }

        [Benchmark]
        public void Type_58_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ClientValidatorCache));
        }

        [Benchmark]
        public void Type_58_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ClientValidatorCache));
        }


        [Benchmark]
        public void Type_59_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ParameterBinder));
        }

        [Benchmark]
        public void Type_59_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ParameterBinder));
        }

        [Benchmark]
        public void Type_59_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ParameterBinder));
        }


        [Benchmark]
        public void Type_60_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(MvcMarkerService));
        }

        [Benchmark]
        public void Type_60_StructureMap()
        {
            var value = Providers[1].GetService(typeof(MvcMarkerService));
        }

        [Benchmark]
        public void Type_60_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(MvcMarkerService));
        }


        [Benchmark]
        public void Type_61_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ITypeActivatorCache));
        }

        [Benchmark]
        public void Type_61_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ITypeActivatorCache));
        }

        [Benchmark]
        public void Type_61_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ITypeActivatorCache));
        }


        [Benchmark]
        public void Type_62_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IUrlHelperFactory));
        }

        [Benchmark]
        public void Type_62_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IUrlHelperFactory));
        }

        [Benchmark]
        public void Type_62_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IUrlHelperFactory));
        }


        [Benchmark]
        public void Type_63_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IHttpRequestStreamReaderFactory));
        }

        [Benchmark]
        public void Type_63_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IHttpRequestStreamReaderFactory));
        }

        [Benchmark]
        public void Type_63_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IHttpRequestStreamReaderFactory));
        }


        [Benchmark]
        public void Type_64_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IHttpResponseStreamWriterFactory));
        }

        [Benchmark]
        public void Type_64_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IHttpResponseStreamWriterFactory));
        }

        [Benchmark]
        public void Type_64_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IHttpResponseStreamWriterFactory));
        }


        [Benchmark]
        public void Type_65_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ArrayPool<byte>));
        }

        [Benchmark]
        public void Type_65_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ArrayPool<byte>));
        }

        [Benchmark]
        public void Type_65_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ArrayPool<byte>));
        }


        [Benchmark]
        public void Type_66_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ArrayPool<char>));
        }

        [Benchmark]
        public void Type_66_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ArrayPool<char>));
        }

        [Benchmark]
        public void Type_66_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ArrayPool<char>));
        }


        [Benchmark]
        public void Type_67_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ObjectResultExecutor));
        }

        [Benchmark]
        public void Type_67_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ObjectResultExecutor));
        }

        [Benchmark]
        public void Type_67_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ObjectResultExecutor));
        }


        [Benchmark]
        public void Type_68_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(PhysicalFileResultExecutor));
        }

        [Benchmark]
        public void Type_68_StructureMap()
        {
            var value = Providers[1].GetService(typeof(PhysicalFileResultExecutor));
        }

        [Benchmark]
        public void Type_68_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(PhysicalFileResultExecutor));
        }


        [Benchmark]
        public void Type_69_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(VirtualFileResultExecutor));
        }

        [Benchmark]
        public void Type_69_StructureMap()
        {
            var value = Providers[1].GetService(typeof(VirtualFileResultExecutor));
        }

        [Benchmark]
        public void Type_69_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(VirtualFileResultExecutor));
        }


        [Benchmark]
        public void Type_70_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(FileStreamResultExecutor));
        }

        [Benchmark]
        public void Type_70_StructureMap()
        {
            var value = Providers[1].GetService(typeof(FileStreamResultExecutor));
        }

        [Benchmark]
        public void Type_70_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(FileStreamResultExecutor));
        }


        [Benchmark]
        public void Type_71_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(FileContentResultExecutor));
        }

        [Benchmark]
        public void Type_71_StructureMap()
        {
            var value = Providers[1].GetService(typeof(FileContentResultExecutor));
        }

        [Benchmark]
        public void Type_71_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(FileContentResultExecutor));
        }


        [Benchmark]
        public void Type_72_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RedirectResultExecutor));
        }

        [Benchmark]
        public void Type_72_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RedirectResultExecutor));
        }

        [Benchmark]
        public void Type_72_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RedirectResultExecutor));
        }


        [Benchmark]
        public void Type_73_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(LocalRedirectResultExecutor));
        }

        [Benchmark]
        public void Type_73_StructureMap()
        {
            var value = Providers[1].GetService(typeof(LocalRedirectResultExecutor));
        }

        [Benchmark]
        public void Type_73_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(LocalRedirectResultExecutor));
        }


        [Benchmark]
        public void Type_74_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RedirectToActionResultExecutor));
        }

        [Benchmark]
        public void Type_74_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RedirectToActionResultExecutor));
        }

        [Benchmark]
        public void Type_74_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RedirectToActionResultExecutor));
        }


        [Benchmark]
        public void Type_75_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RedirectToRouteResultExecutor));
        }

        [Benchmark]
        public void Type_75_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RedirectToRouteResultExecutor));
        }

        [Benchmark]
        public void Type_75_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RedirectToRouteResultExecutor));
        }


        [Benchmark]
        public void Type_76_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RedirectToPageResultExecutor));
        }

        [Benchmark]
        public void Type_76_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RedirectToPageResultExecutor));
        }

        [Benchmark]
        public void Type_76_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RedirectToPageResultExecutor));
        }


        [Benchmark]
        public void Type_77_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ContentResultExecutor));
        }

        [Benchmark]
        public void Type_77_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ContentResultExecutor));
        }

        [Benchmark]
        public void Type_77_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ContentResultExecutor));
        }


        [Benchmark]
        public void Type_78_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(MvcRouteHandler));
        }

        [Benchmark]
        public void Type_78_StructureMap()
        {
            var value = Providers[1].GetService(typeof(MvcRouteHandler));
        }

        [Benchmark]
        public void Type_78_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(MvcRouteHandler));
        }


        [Benchmark]
        public void Type_79_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(MvcAttributeRouteHandler));
        }

        [Benchmark]
        public void Type_79_StructureMap()
        {
            var value = Providers[1].GetService(typeof(MvcAttributeRouteHandler));
        }

        [Benchmark]
        public void Type_79_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(MvcAttributeRouteHandler));
        }


        [Benchmark]
        public void Type_80_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(MiddlewareFilterConfigurationProvider));
        }

        [Benchmark]
        public void Type_80_StructureMap()
        {
            var value = Providers[1].GetService(typeof(MiddlewareFilterConfigurationProvider));
        }

        [Benchmark]
        public void Type_80_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(MiddlewareFilterConfigurationProvider));
        }


        [Benchmark]
        public void Type_81_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(MiddlewareFilterBuilder));
        }

        [Benchmark]
        public void Type_81_StructureMap()
        {
            var value = Providers[1].GetService(typeof(MiddlewareFilterBuilder));
        }

        [Benchmark]
        public void Type_81_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(MiddlewareFilterBuilder));
        }


        [Benchmark]
        public void Type_82_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApiDescriptionGroupCollectionProvider));
        }

        [Benchmark]
        public void Type_82_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApiDescriptionGroupCollectionProvider));
        }

        [Benchmark]
        public void Type_82_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApiDescriptionGroupCollectionProvider));
        }


        [Benchmark]
        public void Type_83_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApiDescriptionProvider));
        }

        [Benchmark]
        public void Type_83_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApiDescriptionProvider));
        }

        [Benchmark]
        public void Type_83_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApiDescriptionProvider));
        }


        [Benchmark]
        public void Type_84_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApiDescriptionProvider));
        }

        [Benchmark]
        public void Type_84_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApiDescriptionProvider));
        }

        [Benchmark]
        public void Type_84_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApiDescriptionProvider));
        }


        [Benchmark]
        public void Type_85_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthenticationService));
        }

        [Benchmark]
        public void Type_85_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthenticationService));
        }

        [Benchmark]
        public void Type_85_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthenticationService));
        }


        [Benchmark]
        public void Type_86_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IClaimsTransformation));
        }

        [Benchmark]
        public void Type_86_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IClaimsTransformation));
        }

        [Benchmark]
        public void Type_86_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IClaimsTransformation));
        }


        [Benchmark]
        public void Type_87_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthenticationHandlerProvider));
        }

        [Benchmark]
        public void Type_87_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthenticationHandlerProvider));
        }

        [Benchmark]
        public void Type_87_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthenticationHandlerProvider));
        }


        [Benchmark]
        public void Type_88_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthenticationSchemeProvider));
        }

        [Benchmark]
        public void Type_88_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthenticationSchemeProvider));
        }

        [Benchmark]
        public void Type_88_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthenticationSchemeProvider));
        }


        [Benchmark]
        public void Type_89_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthorizationService));
        }

        [Benchmark]
        public void Type_89_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthorizationService));
        }

        [Benchmark]
        public void Type_89_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthorizationService));
        }


        [Benchmark]
        public void Type_90_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthorizationPolicyProvider));
        }

        [Benchmark]
        public void Type_90_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthorizationPolicyProvider));
        }

        [Benchmark]
        public void Type_90_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthorizationPolicyProvider));
        }


        [Benchmark]
        public void Type_91_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthorizationHandlerProvider));
        }

        [Benchmark]
        public void Type_91_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthorizationHandlerProvider));
        }

        [Benchmark]
        public void Type_91_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthorizationHandlerProvider));
        }


        [Benchmark]
        public void Type_92_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthorizationEvaluator));
        }

        [Benchmark]
        public void Type_92_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthorizationEvaluator));
        }

        [Benchmark]
        public void Type_92_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthorizationEvaluator));
        }


        [Benchmark]
        public void Type_93_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthorizationHandlerContextFactory));
        }

        [Benchmark]
        public void Type_93_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthorizationHandlerContextFactory));
        }

        [Benchmark]
        public void Type_93_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthorizationHandlerContextFactory));
        }


        [Benchmark]
        public void Type_94_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAuthorizationHandler));
        }

        [Benchmark]
        public void Type_94_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAuthorizationHandler));
        }

        [Benchmark]
        public void Type_94_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAuthorizationHandler));
        }


        [Benchmark]
        public void Type_95_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPolicyEvaluator));
        }

        [Benchmark]
        public void Type_95_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPolicyEvaluator));
        }

        [Benchmark]
        public void Type_95_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPolicyEvaluator));
        }


        [Benchmark]
        public void Type_96_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(FormatFilter));
        }

        [Benchmark]
        public void Type_96_StructureMap()
        {
            var value = Providers[1].GetService(typeof(FormatFilter));
        }

        [Benchmark]
        public void Type_96_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(FormatFilter));
        }


        [Benchmark]
        public void Type_97_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IValidationAttributeAdapterProvider));
        }

        [Benchmark]
        public void Type_97_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IValidationAttributeAdapterProvider));
        }

        [Benchmark]
        public void Type_97_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IValidationAttributeAdapterProvider));
        }


        [Benchmark]
        public void Type_98_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActivator));
        }

        [Benchmark]
        public void Type_98_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActivator));
        }

        [Benchmark]
        public void Type_98_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActivator));
        }


        [Benchmark]
        public void Type_99_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<KeyManagementOptions>));
        }

        [Benchmark]
        public void Type_99_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<KeyManagementOptions>));
        }

        [Benchmark]
        public void Type_99_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<KeyManagementOptions>));
        }


        [Benchmark]
        public void Type_100_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<DataProtectionOptions>));
        }

        [Benchmark]
        public void Type_100_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<DataProtectionOptions>));
        }

        [Benchmark]
        public void Type_100_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<DataProtectionOptions>));
        }


        [Benchmark]
        public void Type_101_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IKeyManager));
        }

        [Benchmark]
        public void Type_101_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IKeyManager));
        }

        [Benchmark]
        public void Type_101_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IKeyManager));
        }


        [Benchmark]
        public void Type_102_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IApplicationDiscriminator));
        }

        [Benchmark]
        public void Type_102_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IApplicationDiscriminator));
        }

        [Benchmark]
        public void Type_102_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IApplicationDiscriminator));
        }


        [Benchmark]
        public void Type_103_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IDefaultKeyResolver));
        }

        [Benchmark]
        public void Type_103_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IDefaultKeyResolver));
        }

        [Benchmark]
        public void Type_103_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IDefaultKeyResolver));
        }


        [Benchmark]
        public void Type_104_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IKeyRingProvider));
        }

        [Benchmark]
        public void Type_104_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IKeyRingProvider));
        }

        [Benchmark]
        public void Type_104_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IKeyRingProvider));
        }


        [Benchmark]
        public void Type_105_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IDataProtectionProvider));
        }

        [Benchmark]
        public void Type_105_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IDataProtectionProvider));
        }

        [Benchmark]
        public void Type_105_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IDataProtectionProvider));
        }


        [Benchmark]
        public void Type_106_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ICertificateResolver));
        }

        [Benchmark]
        public void Type_106_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ICertificateResolver));
        }

        [Benchmark]
        public void Type_106_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ICertificateResolver));
        }


        [Benchmark]
        public void Type_107_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<AntiforgeryOptions>));
        }

        [Benchmark]
        public void Type_107_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<AntiforgeryOptions>));
        }

        [Benchmark]
        public void Type_107_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<AntiforgeryOptions>));
        }


        [Benchmark]
        public void Type_108_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAntiforgery));
        }

        [Benchmark]
        public void Type_108_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAntiforgery));
        }

        [Benchmark]
        public void Type_108_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAntiforgery));
        }


        [Benchmark]
        public void Type_109_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAntiforgeryTokenGenerator));
        }

        [Benchmark]
        public void Type_109_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAntiforgeryTokenGenerator));
        }

        [Benchmark]
        public void Type_109_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAntiforgeryTokenGenerator));
        }


        [Benchmark]
        public void Type_110_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAntiforgeryTokenSerializer));
        }

        [Benchmark]
        public void Type_110_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAntiforgeryTokenSerializer));
        }

        [Benchmark]
        public void Type_110_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAntiforgeryTokenSerializer));
        }


        [Benchmark]
        public void Type_111_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAntiforgeryTokenStore));
        }

        [Benchmark]
        public void Type_111_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAntiforgeryTokenStore));
        }

        [Benchmark]
        public void Type_111_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAntiforgeryTokenStore));
        }


        [Benchmark]
        public void Type_112_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IClaimUidExtractor));
        }

        [Benchmark]
        public void Type_112_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IClaimUidExtractor));
        }

        [Benchmark]
        public void Type_112_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IClaimUidExtractor));
        }


        [Benchmark]
        public void Type_113_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IAntiforgeryAdditionalDataProvider));
        }

        [Benchmark]
        public void Type_113_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IAntiforgeryAdditionalDataProvider));
        }

        [Benchmark]
        public void Type_113_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IAntiforgeryAdditionalDataProvider));
        }


        [Benchmark]
        public void Type_114_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ObjectPool<AntiforgerySerializationContext>));
        }

        [Benchmark]
        public void Type_114_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ObjectPool<AntiforgerySerializationContext>));
        }

        [Benchmark]
        public void Type_114_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ObjectPool<AntiforgerySerializationContext>));
        }


        [Benchmark]
        public void Type_115_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(HtmlEncoder));
        }

        [Benchmark]
        public void Type_115_StructureMap()
        {
            var value = Providers[1].GetService(typeof(HtmlEncoder));
        }

        [Benchmark]
        public void Type_115_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(HtmlEncoder));
        }


        [Benchmark]
        public void Type_116_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(JavaScriptEncoder));
        }

        [Benchmark]
        public void Type_116_StructureMap()
        {
            var value = Providers[1].GetService(typeof(JavaScriptEncoder));
        }

        [Benchmark]
        public void Type_116_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(JavaScriptEncoder));
        }


        [Benchmark]
        public void Type_117_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_117_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_117_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<MvcViewOptions>));
        }


        [Benchmark]
        public void Type_118_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_118_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_118_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<MvcViewOptions>));
        }


        [Benchmark]
        public void Type_119_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ICompositeViewEngine));
        }

        [Benchmark]
        public void Type_119_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ICompositeViewEngine));
        }

        [Benchmark]
        public void Type_119_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ICompositeViewEngine));
        }


        [Benchmark]
        public void Type_120_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ViewResultExecutor));
        }

        [Benchmark]
        public void Type_120_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ViewResultExecutor));
        }

        [Benchmark]
        public void Type_120_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ViewResultExecutor));
        }


        [Benchmark]
        public void Type_121_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(PartialViewResultExecutor));
        }

        [Benchmark]
        public void Type_121_StructureMap()
        {
            var value = Providers[1].GetService(typeof(PartialViewResultExecutor));
        }

        [Benchmark]
        public void Type_121_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(PartialViewResultExecutor));
        }


        [Benchmark]
        public void Type_122_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IHtmlHelper));
        }

        [Benchmark]
        public void Type_122_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IHtmlHelper));
        }

        [Benchmark]
        public void Type_122_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IHtmlHelper));
        }


        [Benchmark]
        public void Type_123_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IHtmlGenerator));
        }

        [Benchmark]
        public void Type_123_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IHtmlGenerator));
        }

        [Benchmark]
        public void Type_123_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IHtmlGenerator));
        }


        [Benchmark]
        public void Type_124_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ExpressionTextCache));
        }

        [Benchmark]
        public void Type_124_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ExpressionTextCache));
        }

        [Benchmark]
        public void Type_124_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ExpressionTextCache));
        }


        [Benchmark]
        public void Type_125_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IModelExpressionProvider));
        }

        [Benchmark]
        public void Type_125_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IModelExpressionProvider));
        }

        [Benchmark]
        public void Type_125_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IModelExpressionProvider));
        }


        [Benchmark]
        public void Type_126_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ValidationHtmlAttributeProvider));
        }

        [Benchmark]
        public void Type_126_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ValidationHtmlAttributeProvider));
        }

        [Benchmark]
        public void Type_126_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ValidationHtmlAttributeProvider));
        }


        [Benchmark]
        public void Type_127_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IJsonHelper));
        }

        [Benchmark]
        public void Type_127_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IJsonHelper));
        }

        [Benchmark]
        public void Type_127_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IJsonHelper));
        }


        [Benchmark]
        public void Type_128_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(JsonOutputFormatter));
        }

        [Benchmark]
        public void Type_128_StructureMap()
        {
            var value = Providers[1].GetService(typeof(JsonOutputFormatter));
        }

        [Benchmark]
        public void Type_128_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(JsonOutputFormatter));
        }


        [Benchmark]
        public void Type_129_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentSelector));
        }

        [Benchmark]
        public void Type_129_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentSelector));
        }

        [Benchmark]
        public void Type_129_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentSelector));
        }


        [Benchmark]
        public void Type_130_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentFactory));
        }

        [Benchmark]
        public void Type_130_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentFactory));
        }

        [Benchmark]
        public void Type_130_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentFactory));
        }


        [Benchmark]
        public void Type_131_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentActivator));
        }

        [Benchmark]
        public void Type_131_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentActivator));
        }

        [Benchmark]
        public void Type_131_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentActivator));
        }


        [Benchmark]
        public void Type_132_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentDescriptorCollectionProvider));
        }

        [Benchmark]
        public void Type_132_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentDescriptorCollectionProvider));
        }

        [Benchmark]
        public void Type_132_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentDescriptorCollectionProvider));
        }


        [Benchmark]
        public void Type_133_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ViewComponentResultExecutor));
        }

        [Benchmark]
        public void Type_133_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ViewComponentResultExecutor));
        }

        [Benchmark]
        public void Type_133_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ViewComponentResultExecutor));
        }


        [Benchmark]
        public void Type_134_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ViewComponentInvokerCache));
        }

        [Benchmark]
        public void Type_134_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ViewComponentInvokerCache));
        }

        [Benchmark]
        public void Type_134_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ViewComponentInvokerCache));
        }


        [Benchmark]
        public void Type_135_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentDescriptorProvider));
        }

        [Benchmark]
        public void Type_135_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentDescriptorProvider));
        }

        [Benchmark]
        public void Type_135_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentDescriptorProvider));
        }


        [Benchmark]
        public void Type_136_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentInvokerFactory));
        }

        [Benchmark]
        public void Type_136_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentInvokerFactory));
        }

        [Benchmark]
        public void Type_136_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentInvokerFactory));
        }


        [Benchmark]
        public void Type_137_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewComponentHelper));
        }

        [Benchmark]
        public void Type_137_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewComponentHelper));
        }

        [Benchmark]
        public void Type_137_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewComponentHelper));
        }


        [Benchmark]
        public void Type_138_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(SaveTempDataFilter));
        }

        [Benchmark]
        public void Type_138_StructureMap()
        {
            var value = Providers[1].GetService(typeof(SaveTempDataFilter));
        }

        [Benchmark]
        public void Type_138_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(SaveTempDataFilter));
        }


        [Benchmark]
        public void Type_139_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ControllerSaveTempDataPropertyFilter));
        }

        [Benchmark]
        public void Type_139_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ControllerSaveTempDataPropertyFilter));
        }

        [Benchmark]
        public void Type_139_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ControllerSaveTempDataPropertyFilter));
        }


        [Benchmark]
        public void Type_140_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ITempDataProvider));
        }

        [Benchmark]
        public void Type_140_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ITempDataProvider));
        }

        [Benchmark]
        public void Type_140_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ITempDataProvider));
        }


        [Benchmark]
        public void Type_141_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ValidateAntiforgeryTokenAuthorizationFilter));
        }

        [Benchmark]
        public void Type_141_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ValidateAntiforgeryTokenAuthorizationFilter));
        }

        [Benchmark]
        public void Type_141_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ValidateAntiforgeryTokenAuthorizationFilter));
        }


        [Benchmark]
        public void Type_142_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(AutoValidateAntiforgeryTokenAuthorizationFilter));
        }

        [Benchmark]
        public void Type_142_StructureMap()
        {
            var value = Providers[1].GetService(typeof(AutoValidateAntiforgeryTokenAuthorizationFilter));
        }

        [Benchmark]
        public void Type_142_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(AutoValidateAntiforgeryTokenAuthorizationFilter));
        }


        [Benchmark]
        public void Type_143_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ITempDataDictionaryFactory));
        }

        [Benchmark]
        public void Type_143_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ITempDataDictionaryFactory));
        }

        [Benchmark]
        public void Type_143_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ITempDataDictionaryFactory));
        }


        [Benchmark]
        public void Type_144_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ArrayPool<ViewBufferValue>));
        }

        [Benchmark]
        public void Type_144_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ArrayPool<ViewBufferValue>));
        }

        [Benchmark]
        public void Type_144_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ArrayPool<ViewBufferValue>));
        }


        [Benchmark]
        public void Type_145_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewBufferScope));
        }

        [Benchmark]
        public void Type_145_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewBufferScope));
        }

        [Benchmark]
        public void Type_145_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewBufferScope));
        }


        [Benchmark]
        public void Type_146_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(CSharpCompiler));
        }

        [Benchmark]
        public void Type_146_StructureMap()
        {
            var value = Providers[1].GetService(typeof(CSharpCompiler));
        }

        [Benchmark]
        public void Type_146_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(CSharpCompiler));
        }


        [Benchmark]
        public void Type_147_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RazorReferenceManager));
        }

        [Benchmark]
        public void Type_147_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RazorReferenceManager));
        }

        [Benchmark]
        public void Type_147_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RazorReferenceManager));
        }


        [Benchmark]
        public void Type_148_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_148_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_148_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<RazorViewEngineOptions>));
        }


        [Benchmark]
        public void Type_149_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IConfigureOptions<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_149_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IConfigureOptions<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_149_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IConfigureOptions<RazorViewEngineOptions>));
        }


        [Benchmark]
        public void Type_150_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IRazorViewEngineFileProviderAccessor));
        }

        [Benchmark]
        public void Type_150_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IRazorViewEngineFileProviderAccessor));
        }

        [Benchmark]
        public void Type_150_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IRazorViewEngineFileProviderAccessor));
        }


        [Benchmark]
        public void Type_151_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IRazorViewEngine));
        }

        [Benchmark]
        public void Type_151_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IRazorViewEngine));
        }

        [Benchmark]
        public void Type_151_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IRazorViewEngine));
        }


        [Benchmark]
        public void Type_152_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IViewCompilerProvider));
        }

        [Benchmark]
        public void Type_152_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IViewCompilerProvider));
        }

        [Benchmark]
        public void Type_152_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IViewCompilerProvider));
        }


        [Benchmark]
        public void Type_153_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IRazorPageFactoryProvider));
        }

        [Benchmark]
        public void Type_153_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IRazorPageFactoryProvider));
        }

        [Benchmark]
        public void Type_153_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IRazorPageFactoryProvider));
        }


        [Benchmark]
        public void Type_154_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RazorProject));
        }

        [Benchmark]
        public void Type_154_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RazorProject));
        }

        [Benchmark]
        public void Type_154_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RazorProject));
        }


        [Benchmark]
        public void Type_155_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RazorTemplateEngine));
        }

        [Benchmark]
        public void Type_155_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RazorTemplateEngine));
        }

        [Benchmark]
        public void Type_155_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RazorTemplateEngine));
        }


        [Benchmark]
        public void Type_156_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(LazyMetadataReferenceFeature));
        }

        [Benchmark]
        public void Type_156_StructureMap()
        {
            var value = Providers[1].GetService(typeof(LazyMetadataReferenceFeature));
        }

        [Benchmark]
        public void Type_156_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(LazyMetadataReferenceFeature));
        }


        [Benchmark]
        public void Type_157_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(RazorEngine));
        }

        [Benchmark]
        public void Type_157_StructureMap()
        {
            var value = Providers[1].GetService(typeof(RazorEngine));
        }

        [Benchmark]
        public void Type_157_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(RazorEngine));
        }


        [Benchmark]
        public void Type_158_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IRazorPageActivator));
        }

        [Benchmark]
        public void Type_158_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IRazorPageActivator));
        }

        [Benchmark]
        public void Type_158_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IRazorPageActivator));
        }


        [Benchmark]
        public void Type_159_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ITagHelperActivator));
        }

        [Benchmark]
        public void Type_159_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ITagHelperActivator));
        }

        [Benchmark]
        public void Type_159_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ITagHelperActivator));
        }


        [Benchmark]
        public void Type_160_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ITagHelperFactory));
        }

        [Benchmark]
        public void Type_160_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ITagHelperFactory));
        }

        [Benchmark]
        public void Type_160_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ITagHelperFactory));
        }


        [Benchmark]
        public void Type_161_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ITagHelperComponentManager));
        }

        [Benchmark]
        public void Type_161_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ITagHelperComponentManager));
        }

        [Benchmark]
        public void Type_161_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ITagHelperComponentManager));
        }


        [Benchmark]
        public void Type_162_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IMemoryCache));
        }

        [Benchmark]
        public void Type_162_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IMemoryCache));
        }

        [Benchmark]
        public void Type_162_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IMemoryCache));
        }


        [Benchmark]
        public void Type_163_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IActionDescriptorChangeProvider));
        }

        [Benchmark]
        public void Type_163_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IActionDescriptorChangeProvider));
        }

        [Benchmark]
        public void Type_163_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IActionDescriptorChangeProvider));
        }


        [Benchmark]
        public void Type_164_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageRouteModelProvider));
        }

        [Benchmark]
        public void Type_164_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageRouteModelProvider));
        }

        [Benchmark]
        public void Type_164_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageRouteModelProvider));
        }


        [Benchmark]
        public void Type_165_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageRouteModelProvider));
        }

        [Benchmark]
        public void Type_165_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageRouteModelProvider));
        }

        [Benchmark]
        public void Type_165_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageRouteModelProvider));
        }


        [Benchmark]
        public void Type_166_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_166_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_166_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageApplicationModelProvider));
        }


        [Benchmark]
        public void Type_167_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_167_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_167_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageApplicationModelProvider));
        }


        [Benchmark]
        public void Type_168_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_168_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_168_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageApplicationModelProvider));
        }


        [Benchmark]
        public void Type_169_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_169_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageApplicationModelProvider));
        }

        [Benchmark]
        public void Type_169_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageApplicationModelProvider));
        }


        [Benchmark]
        public void Type_170_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageModelActivatorProvider));
        }

        [Benchmark]
        public void Type_170_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageModelActivatorProvider));
        }

        [Benchmark]
        public void Type_170_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageModelActivatorProvider));
        }


        [Benchmark]
        public void Type_171_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageModelFactoryProvider));
        }

        [Benchmark]
        public void Type_171_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageModelFactoryProvider));
        }

        [Benchmark]
        public void Type_171_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageModelFactoryProvider));
        }


        [Benchmark]
        public void Type_172_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageActivatorProvider));
        }

        [Benchmark]
        public void Type_172_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageActivatorProvider));
        }

        [Benchmark]
        public void Type_172_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageActivatorProvider));
        }


        [Benchmark]
        public void Type_173_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageFactoryProvider));
        }

        [Benchmark]
        public void Type_173_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageFactoryProvider));
        }

        [Benchmark]
        public void Type_173_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageFactoryProvider));
        }


        [Benchmark]
        public void Type_174_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageLoader));
        }

        [Benchmark]
        public void Type_174_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageLoader));
        }

        [Benchmark]
        public void Type_174_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageLoader));
        }


        [Benchmark]
        public void Type_175_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IPageHandlerMethodSelector));
        }

        [Benchmark]
        public void Type_175_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IPageHandlerMethodSelector));
        }

        [Benchmark]
        public void Type_175_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IPageHandlerMethodSelector));
        }


        [Benchmark]
        public void Type_176_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(PageArgumentBinder));
        }

        [Benchmark]
        public void Type_176_StructureMap()
        {
            var value = Providers[1].GetService(typeof(PageArgumentBinder));
        }

        [Benchmark]
        public void Type_176_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(PageArgumentBinder));
        }


        [Benchmark]
        public void Type_177_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(PageResultExecutor));
        }

        [Benchmark]
        public void Type_177_StructureMap()
        {
            var value = Providers[1].GetService(typeof(PageResultExecutor));
        }

        [Benchmark]
        public void Type_177_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(PageResultExecutor));
        }


        [Benchmark]
        public void Type_178_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(PageSaveTempDataPropertyFilter));
        }

        [Benchmark]
        public void Type_178_StructureMap()
        {
            var value = Providers[1].GetService(typeof(PageSaveTempDataPropertyFilter));
        }

        [Benchmark]
        public void Type_178_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(PageSaveTempDataPropertyFilter));
        }


        [Benchmark]
        public void Type_179_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IDistributedCacheTagHelperStorage));
        }

        [Benchmark]
        public void Type_179_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IDistributedCacheTagHelperStorage));
        }

        [Benchmark]
        public void Type_179_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IDistributedCacheTagHelperStorage));
        }


        [Benchmark]
        public void Type_180_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IDistributedCacheTagHelperFormatter));
        }

        [Benchmark]
        public void Type_180_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IDistributedCacheTagHelperFormatter));
        }

        [Benchmark]
        public void Type_180_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IDistributedCacheTagHelperFormatter));
        }


        [Benchmark]
        public void Type_181_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IDistributedCacheTagHelperService));
        }

        [Benchmark]
        public void Type_181_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IDistributedCacheTagHelperService));
        }

        [Benchmark]
        public void Type_181_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IDistributedCacheTagHelperService));
        }


        [Benchmark]
        public void Type_182_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IDistributedCache));
        }

        [Benchmark]
        public void Type_182_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IDistributedCache));
        }

        [Benchmark]
        public void Type_182_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IDistributedCache));
        }


        [Benchmark]
        public void Type_183_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(CacheTagHelperMemoryCacheFactory));
        }

        [Benchmark]
        public void Type_183_StructureMap()
        {
            var value = Providers[1].GetService(typeof(CacheTagHelperMemoryCacheFactory));
        }

        [Benchmark]
        public void Type_183_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(CacheTagHelperMemoryCacheFactory));
        }


        [Benchmark]
        public void Type_184_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(JsonResultExecutor));
        }

        [Benchmark]
        public void Type_184_StructureMap()
        {
            var value = Providers[1].GetService(typeof(JsonResultExecutor));
        }

        [Benchmark]
        public void Type_184_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(JsonResultExecutor));
        }


        [Benchmark]
        public void Type_185_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ICorsService));
        }

        [Benchmark]
        public void Type_185_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ICorsService));
        }

        [Benchmark]
        public void Type_185_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ICorsService));
        }


        [Benchmark]
        public void Type_186_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ICorsPolicyProvider));
        }

        [Benchmark]
        public void Type_186_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ICorsPolicyProvider));
        }

        [Benchmark]
        public void Type_186_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ICorsPolicyProvider));
        }


        [Benchmark]
        public void Type_187_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(CorsAuthorizationFilter));
        }

        [Benchmark]
        public void Type_187_StructureMap()
        {
            var value = Providers[1].GetService(typeof(CorsAuthorizationFilter));
        }

        [Benchmark]
        public void Type_187_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(CorsAuthorizationFilter));
        }


        [Benchmark]
        public void Type_188_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IServiceProvider));
        }

        [Benchmark]
        public void Type_188_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IServiceProvider));
        }

        [Benchmark]
        public void Type_188_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IServiceProvider));
        }


        [Benchmark]
        public void Type_189_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IServiceScopeFactory));
        }

        [Benchmark]
        public void Type_189_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IServiceScopeFactory));
        }

        [Benchmark]
        public void Type_189_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IServiceScopeFactory));
        }


        [Benchmark]
        public void Type_190_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<FormOptions>));
        }

        [Benchmark]
        public void Type_190_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<FormOptions>));
        }

        [Benchmark]
        public void Type_190_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<FormOptions>));
        }


        [Benchmark]
        public void Type_191_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<FormOptions>));
        }

        [Benchmark]
        public void Type_191_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<FormOptions>));
        }

        [Benchmark]
        public void Type_191_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<FormOptions>));
        }


        [Benchmark]
        public void Type_192_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<FormOptions>>));
        }

        [Benchmark]
        public void Type_192_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<FormOptions>>));
        }

        [Benchmark]
        public void Type_192_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<FormOptions>>));
        }


        [Benchmark]
        public void Type_193_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<FormOptions>>));
        }

        [Benchmark]
        public void Type_193_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<FormOptions>>));
        }

        [Benchmark]
        public void Type_193_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<FormOptions>>));
        }


        [Benchmark]
        public void Type_194_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<ILoggerProvider>));
        }

        [Benchmark]
        public void Type_194_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<ILoggerProvider>));
        }

        [Benchmark]
        public void Type_194_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<ILoggerProvider>));
        }


        [Benchmark]
        public void Type_195_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(LoggerFilterOptions));
        }

        [Benchmark]
        public void Type_195_StructureMap()
        {
            var value = Providers[1].GetService(typeof(LoggerFilterOptions));
        }

        [Benchmark]
        public void Type_195_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(LoggerFilterOptions));
        }


        [Benchmark]
        public void Type_196_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsMonitor<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_196_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsMonitor<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_196_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsMonitor<LoggerFilterOptions>));
        }


        [Benchmark]
        public void Type_197_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<KeyManagementOptions>));
        }

        [Benchmark]
        public void Type_197_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<KeyManagementOptions>));
        }

        [Benchmark]
        public void Type_197_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<KeyManagementOptions>));
        }


        [Benchmark]
        public void Type_198_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<KeyManagementOptions>));
        }

        [Benchmark]
        public void Type_198_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<KeyManagementOptions>));
        }

        [Benchmark]
        public void Type_198_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<KeyManagementOptions>));
        }


        [Benchmark]
        public void Type_199_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<KeyManagementOptions>>));
        }

        [Benchmark]
        public void Type_199_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<KeyManagementOptions>>));
        }

        [Benchmark]
        public void Type_199_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<KeyManagementOptions>>));
        }


        [Benchmark]
        public void Type_200_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<KeyManagementOptions>>));
        }

        [Benchmark]
        public void Type_200_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<KeyManagementOptions>>));
        }

        [Benchmark]
        public void Type_200_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<KeyManagementOptions>>));
        }


        [Benchmark]
        public void Type_201_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ServiceProviderOptions));
        }

        [Benchmark]
        public void Type_201_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ServiceProviderOptions));
        }

        [Benchmark]
        public void Type_201_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ServiceProviderOptions));
        }


        [Benchmark]
        public void Type_202_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILogger<ApplicationLifetime>));
        }

        [Benchmark]
        public void Type_202_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILogger<ApplicationLifetime>));
        }

        [Benchmark]
        public void Type_202_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILogger<ApplicationLifetime>));
        }


        [Benchmark]
        public void Type_203_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILogger<HostedServiceExecutor>));
        }

        [Benchmark]
        public void Type_203_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILogger<HostedServiceExecutor>));
        }

        [Benchmark]
        public void Type_203_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILogger<HostedServiceExecutor>));
        }


        [Benchmark]
        public void Type_204_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IHostedService>));
        }

        [Benchmark]
        public void Type_204_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IHostedService>));
        }

        [Benchmark]
        public void Type_204_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IHostedService>));
        }


        [Benchmark]
        public void Type_205_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<RouteOptions>));
        }

        [Benchmark]
        public void Type_205_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<RouteOptions>));
        }

        [Benchmark]
        public void Type_205_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<RouteOptions>));
        }


        [Benchmark]
        public void Type_206_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<RouteOptions>));
        }

        [Benchmark]
        public void Type_206_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<RouteOptions>));
        }

        [Benchmark]
        public void Type_206_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<RouteOptions>));
        }


        [Benchmark]
        public void Type_207_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<RouteOptions>>));
        }

        [Benchmark]
        public void Type_207_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<RouteOptions>>));
        }

        [Benchmark]
        public void Type_207_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<RouteOptions>>));
        }


        [Benchmark]
        public void Type_208_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<RouteOptions>>));
        }

        [Benchmark]
        public void Type_208_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<RouteOptions>>));
        }

        [Benchmark]
        public void Type_208_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<RouteOptions>>));
        }


        [Benchmark]
        public void Type_209_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<MvcDataAnnotationsLocalizationOptions>));
        }

        [Benchmark]
        public void Type_209_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<MvcDataAnnotationsLocalizationOptions>));
        }

        [Benchmark]
        public void Type_209_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<MvcDataAnnotationsLocalizationOptions>));
        }


        [Benchmark]
        public void Type_210_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<MvcDataAnnotationsLocalizationOptions>));
        }

        [Benchmark]
        public void Type_210_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<MvcDataAnnotationsLocalizationOptions>));
        }

        [Benchmark]
        public void Type_210_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<MvcDataAnnotationsLocalizationOptions>));
        }


        [Benchmark]
        public void Type_211_BlueMilk()
        {
            var value = Providers[0]
                .GetService(typeof(IEnumerable<IConfigureOptions<MvcDataAnnotationsLocalizationOptions>>));
        }

        [Benchmark]
        public void Type_211_StructureMap()
        {
            var value = Providers[1]
                .GetService(typeof(IEnumerable<IConfigureOptions<MvcDataAnnotationsLocalizationOptions>>));
        }

        [Benchmark]
        public void Type_211_AspNetCore()
        {
            var value = Providers[2]
                .GetService(typeof(IEnumerable<IConfigureOptions<MvcDataAnnotationsLocalizationOptions>>));
        }


        [Benchmark]
        public void Type_212_BlueMilk()
        {
            var value = Providers[0]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<MvcDataAnnotationsLocalizationOptions>>));
        }

        [Benchmark]
        public void Type_212_StructureMap()
        {
            var value = Providers[1]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<MvcDataAnnotationsLocalizationOptions>>));
        }

        [Benchmark]
        public void Type_212_AspNetCore()
        {
            var value = Providers[2]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<MvcDataAnnotationsLocalizationOptions>>));
        }


        [Benchmark]
        public void Type_213_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<MvcJsonOptions>));
        }

        [Benchmark]
        public void Type_213_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<MvcJsonOptions>));
        }

        [Benchmark]
        public void Type_213_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<MvcJsonOptions>));
        }


        [Benchmark]
        public void Type_214_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<MvcJsonOptions>));
        }

        [Benchmark]
        public void Type_214_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<MvcJsonOptions>));
        }

        [Benchmark]
        public void Type_214_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<MvcJsonOptions>));
        }


        [Benchmark]
        public void Type_215_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<MvcJsonOptions>>));
        }

        [Benchmark]
        public void Type_215_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<MvcJsonOptions>>));
        }

        [Benchmark]
        public void Type_215_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<MvcJsonOptions>>));
        }


        [Benchmark]
        public void Type_216_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcJsonOptions>>));
        }

        [Benchmark]
        public void Type_216_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcJsonOptions>>));
        }

        [Benchmark]
        public void Type_216_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcJsonOptions>>));
        }


        [Benchmark]
        public void Type_217_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_217_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<MvcOptions>));
        }

        [Benchmark]
        public void Type_217_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<MvcOptions>));
        }


        [Benchmark]
        public void Type_218_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<MvcOptions>));
        }

        [Benchmark]
        public void Type_218_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<MvcOptions>));
        }

        [Benchmark]
        public void Type_218_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<MvcOptions>));
        }


        [Benchmark]
        public void Type_219_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<MvcOptions>>));
        }

        [Benchmark]
        public void Type_219_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<MvcOptions>>));
        }

        [Benchmark]
        public void Type_219_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<MvcOptions>>));
        }


        [Benchmark]
        public void Type_220_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcOptions>>));
        }

        [Benchmark]
        public void Type_220_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcOptions>>));
        }

        [Benchmark]
        public void Type_220_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcOptions>>));
        }


        [Benchmark]
        public void Type_221_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<AuthorizationOptions>));
        }

        [Benchmark]
        public void Type_221_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<AuthorizationOptions>));
        }

        [Benchmark]
        public void Type_221_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<AuthorizationOptions>));
        }


        [Benchmark]
        public void Type_222_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<AuthorizationOptions>));
        }

        [Benchmark]
        public void Type_222_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<AuthorizationOptions>));
        }

        [Benchmark]
        public void Type_222_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<AuthorizationOptions>));
        }


        [Benchmark]
        public void Type_223_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<AuthorizationOptions>>));
        }

        [Benchmark]
        public void Type_223_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<AuthorizationOptions>>));
        }

        [Benchmark]
        public void Type_223_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<AuthorizationOptions>>));
        }


        [Benchmark]
        public void Type_224_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<AuthorizationOptions>>));
        }

        [Benchmark]
        public void Type_224_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<AuthorizationOptions>>));
        }

        [Benchmark]
        public void Type_224_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<AuthorizationOptions>>));
        }


        [Benchmark]
        public void Type_225_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IApplicationModelProvider>));
        }

        [Benchmark]
        public void Type_225_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IApplicationModelProvider>));
        }

        [Benchmark]
        public void Type_225_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IApplicationModelProvider>));
        }


        [Benchmark]
        public void Type_226_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPageRouteModelProvider>));
        }

        [Benchmark]
        public void Type_226_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPageRouteModelProvider>));
        }

        [Benchmark]
        public void Type_226_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPageRouteModelProvider>));
        }


        [Benchmark]
        public void Type_227_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<RazorPagesOptions>));
        }

        [Benchmark]
        public void Type_227_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<RazorPagesOptions>));
        }

        [Benchmark]
        public void Type_227_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<RazorPagesOptions>));
        }


        [Benchmark]
        public void Type_228_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<RazorPagesOptions>));
        }

        [Benchmark]
        public void Type_228_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<RazorPagesOptions>));
        }

        [Benchmark]
        public void Type_228_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<RazorPagesOptions>));
        }


        [Benchmark]
        public void Type_229_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<RazorPagesOptions>>));
        }

        [Benchmark]
        public void Type_229_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<RazorPagesOptions>>));
        }

        [Benchmark]
        public void Type_229_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<RazorPagesOptions>>));
        }


        [Benchmark]
        public void Type_230_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<RazorPagesOptions>>));
        }

        [Benchmark]
        public void Type_230_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<RazorPagesOptions>>));
        }

        [Benchmark]
        public void Type_230_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<RazorPagesOptions>>));
        }


        [Benchmark]
        public void Type_231_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IActionDescriptorProvider>));
        }

        [Benchmark]
        public void Type_231_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IActionDescriptorProvider>));
        }

        [Benchmark]
        public void Type_231_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IActionDescriptorProvider>));
        }


        [Benchmark]
        public void Type_232_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IActionDescriptorChangeProvider>));
        }

        [Benchmark]
        public void Type_232_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IActionDescriptorChangeProvider>));
        }

        [Benchmark]
        public void Type_232_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IActionDescriptorChangeProvider>));
        }


        [Benchmark]
        public void Type_233_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IActionConstraintProvider>));
        }

        [Benchmark]
        public void Type_233_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IActionConstraintProvider>));
        }

        [Benchmark]
        public void Type_233_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IActionConstraintProvider>));
        }


        [Benchmark]
        public void Type_234_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IControllerPropertyActivator>));
        }

        [Benchmark]
        public void Type_234_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IControllerPropertyActivator>));
        }

        [Benchmark]
        public void Type_234_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IControllerPropertyActivator>));
        }


        [Benchmark]
        public void Type_235_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IActionInvokerProvider>));
        }

        [Benchmark]
        public void Type_235_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IActionInvokerProvider>));
        }

        [Benchmark]
        public void Type_235_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IActionInvokerProvider>));
        }


        [Benchmark]
        public void Type_236_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IFilterProvider>));
        }

        [Benchmark]
        public void Type_236_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IFilterProvider>));
        }

        [Benchmark]
        public void Type_236_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IFilterProvider>));
        }


        [Benchmark]
        public void Type_237_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<HtmlHelperOptions>));
        }

        [Benchmark]
        public void Type_237_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<HtmlHelperOptions>));
        }

        [Benchmark]
        public void Type_237_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<HtmlHelperOptions>));
        }


        [Benchmark]
        public void Type_238_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPageApplicationModelProvider>));
        }

        [Benchmark]
        public void Type_238_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPageApplicationModelProvider>));
        }

        [Benchmark]
        public void Type_238_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPageApplicationModelProvider>));
        }


        [Benchmark]
        public void Type_239_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_239_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_239_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<RazorViewEngineOptions>));
        }


        [Benchmark]
        public void Type_240_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_240_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<RazorViewEngineOptions>));
        }

        [Benchmark]
        public void Type_240_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<RazorViewEngineOptions>));
        }


        [Benchmark]
        public void Type_241_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<RazorViewEngineOptions>>));
        }

        [Benchmark]
        public void Type_241_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<RazorViewEngineOptions>>));
        }

        [Benchmark]
        public void Type_241_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<RazorViewEngineOptions>>));
        }


        [Benchmark]
        public void Type_242_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<RazorViewEngineOptions>>));
        }

        [Benchmark]
        public void Type_242_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<RazorViewEngineOptions>>));
        }

        [Benchmark]
        public void Type_242_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<RazorViewEngineOptions>>));
        }


        [Benchmark]
        public void Type_243_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<CookieTempDataProviderOptions>));
        }

        [Benchmark]
        public void Type_243_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<CookieTempDataProviderOptions>));
        }

        [Benchmark]
        public void Type_243_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<CookieTempDataProviderOptions>));
        }


        [Benchmark]
        public void Type_244_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<CookieTempDataProviderOptions>));
        }

        [Benchmark]
        public void Type_244_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<CookieTempDataProviderOptions>));
        }

        [Benchmark]
        public void Type_244_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<CookieTempDataProviderOptions>));
        }


        [Benchmark]
        public void Type_245_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<CookieTempDataProviderOptions>>));
        }

        [Benchmark]
        public void Type_245_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<CookieTempDataProviderOptions>>));
        }

        [Benchmark]
        public void Type_245_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<CookieTempDataProviderOptions>>));
        }


        [Benchmark]
        public void Type_246_BlueMilk()
        {
            var value = Providers[0]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<CookieTempDataProviderOptions>>));
        }

        [Benchmark]
        public void Type_246_StructureMap()
        {
            var value = Providers[1]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<CookieTempDataProviderOptions>>));
        }

        [Benchmark]
        public void Type_246_AspNetCore()
        {
            var value = Providers[2]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<CookieTempDataProviderOptions>>));
        }


        [Benchmark]
        public void Type_247_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<HtmlHelperOptions>));
        }

        [Benchmark]
        public void Type_247_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<HtmlHelperOptions>));
        }

        [Benchmark]
        public void Type_247_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<HtmlHelperOptions>));
        }


        [Benchmark]
        public void Type_248_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<HtmlHelperOptions>>));
        }

        [Benchmark]
        public void Type_248_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<HtmlHelperOptions>>));
        }

        [Benchmark]
        public void Type_248_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<HtmlHelperOptions>>));
        }


        [Benchmark]
        public void Type_249_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<HtmlHelperOptions>>));
        }

        [Benchmark]
        public void Type_249_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<HtmlHelperOptions>>));
        }

        [Benchmark]
        public void Type_249_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<HtmlHelperOptions>>));
        }


        [Benchmark]
        public void Type_250_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILogger<ContentResultExecutor>));
        }

        [Benchmark]
        public void Type_250_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILogger<ContentResultExecutor>));
        }

        [Benchmark]
        public void Type_250_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILogger<ContentResultExecutor>));
        }


        [Benchmark]
        public void Type_251_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IApiDescriptionProvider>));
        }

        [Benchmark]
        public void Type_251_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IApiDescriptionProvider>));
        }

        [Benchmark]
        public void Type_251_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IApiDescriptionProvider>));
        }


        [Benchmark]
        public void Type_252_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<AuthenticationOptions>));
        }

        [Benchmark]
        public void Type_252_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<AuthenticationOptions>));
        }

        [Benchmark]
        public void Type_252_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<AuthenticationOptions>));
        }


        [Benchmark]
        public void Type_253_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<AuthenticationOptions>));
        }

        [Benchmark]
        public void Type_253_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<AuthenticationOptions>));
        }

        [Benchmark]
        public void Type_253_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<AuthenticationOptions>));
        }


        [Benchmark]
        public void Type_254_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<AuthenticationOptions>>));
        }

        [Benchmark]
        public void Type_254_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<AuthenticationOptions>>));
        }

        [Benchmark]
        public void Type_254_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<AuthenticationOptions>>));
        }


        [Benchmark]
        public void Type_255_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<AuthenticationOptions>>));
        }

        [Benchmark]
        public void Type_255_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<AuthenticationOptions>>));
        }

        [Benchmark]
        public void Type_255_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<AuthenticationOptions>>));
        }


        [Benchmark]
        public void Type_256_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILogger<DefaultAuthorizationService>));
        }

        [Benchmark]
        public void Type_256_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILogger<DefaultAuthorizationService>));
        }

        [Benchmark]
        public void Type_256_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILogger<DefaultAuthorizationService>));
        }


        [Benchmark]
        public void Type_257_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IAuthorizationHandler>));
        }

        [Benchmark]
        public void Type_257_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IAuthorizationHandler>));
        }

        [Benchmark]
        public void Type_257_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IAuthorizationHandler>));
        }


        [Benchmark]
        public void Type_258_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<DataProtectionOptions>));
        }

        [Benchmark]
        public void Type_258_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<DataProtectionOptions>));
        }

        [Benchmark]
        public void Type_258_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<DataProtectionOptions>));
        }


        [Benchmark]
        public void Type_259_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<DataProtectionOptions>));
        }

        [Benchmark]
        public void Type_259_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<DataProtectionOptions>));
        }

        [Benchmark]
        public void Type_259_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<DataProtectionOptions>));
        }


        [Benchmark]
        public void Type_260_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<DataProtectionOptions>>));
        }

        [Benchmark]
        public void Type_260_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<DataProtectionOptions>>));
        }

        [Benchmark]
        public void Type_260_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<DataProtectionOptions>>));
        }


        [Benchmark]
        public void Type_261_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<DataProtectionOptions>>));
        }

        [Benchmark]
        public void Type_261_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<DataProtectionOptions>>));
        }

        [Benchmark]
        public void Type_261_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<DataProtectionOptions>>));
        }


        [Benchmark]
        public void Type_262_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<AntiforgeryOptions>));
        }

        [Benchmark]
        public void Type_262_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<AntiforgeryOptions>));
        }

        [Benchmark]
        public void Type_262_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<AntiforgeryOptions>));
        }


        [Benchmark]
        public void Type_263_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<AntiforgeryOptions>));
        }

        [Benchmark]
        public void Type_263_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<AntiforgeryOptions>));
        }

        [Benchmark]
        public void Type_263_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<AntiforgeryOptions>));
        }


        [Benchmark]
        public void Type_264_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<AntiforgeryOptions>>));
        }

        [Benchmark]
        public void Type_264_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<AntiforgeryOptions>>));
        }

        [Benchmark]
        public void Type_264_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<AntiforgeryOptions>>));
        }


        [Benchmark]
        public void Type_265_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<AntiforgeryOptions>>));
        }

        [Benchmark]
        public void Type_265_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<AntiforgeryOptions>>));
        }

        [Benchmark]
        public void Type_265_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<AntiforgeryOptions>>));
        }


        [Benchmark]
        public void Type_266_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_266_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_266_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<MvcViewOptions>));
        }


        [Benchmark]
        public void Type_267_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_267_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<MvcViewOptions>));
        }

        [Benchmark]
        public void Type_267_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<MvcViewOptions>));
        }


        [Benchmark]
        public void Type_268_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<MvcViewOptions>>));
        }

        [Benchmark]
        public void Type_268_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<MvcViewOptions>>));
        }

        [Benchmark]
        public void Type_268_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<MvcViewOptions>>));
        }


        [Benchmark]
        public void Type_269_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcViewOptions>>));
        }

        [Benchmark]
        public void Type_269_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcViewOptions>>));
        }

        [Benchmark]
        public void Type_269_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<MvcViewOptions>>));
        }


        [Benchmark]
        public void Type_270_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<ITagHelperComponent>));
        }

        [Benchmark]
        public void Type_270_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<ITagHelperComponent>));
        }

        [Benchmark]
        public void Type_270_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<ITagHelperComponent>));
        }


        [Benchmark]
        public void Type_271_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<MemoryCacheOptions>));
        }

        [Benchmark]
        public void Type_271_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<MemoryCacheOptions>));
        }

        [Benchmark]
        public void Type_271_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<MemoryCacheOptions>));
        }


        [Benchmark]
        public void Type_272_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<MemoryCacheOptions>));
        }

        [Benchmark]
        public void Type_272_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<MemoryCacheOptions>));
        }

        [Benchmark]
        public void Type_272_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<MemoryCacheOptions>));
        }


        [Benchmark]
        public void Type_273_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<MemoryCacheOptions>>));
        }

        [Benchmark]
        public void Type_273_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<MemoryCacheOptions>>));
        }

        [Benchmark]
        public void Type_273_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<MemoryCacheOptions>>));
        }


        [Benchmark]
        public void Type_274_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<MemoryCacheOptions>>));
        }

        [Benchmark]
        public void Type_274_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<MemoryCacheOptions>>));
        }

        [Benchmark]
        public void Type_274_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<MemoryCacheOptions>>));
        }


        [Benchmark]
        public void Type_275_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<MemoryDistributedCacheOptions>));
        }

        [Benchmark]
        public void Type_275_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<MemoryDistributedCacheOptions>));
        }

        [Benchmark]
        public void Type_275_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<MemoryDistributedCacheOptions>));
        }


        [Benchmark]
        public void Type_276_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<MemoryDistributedCacheOptions>));
        }

        [Benchmark]
        public void Type_276_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<MemoryDistributedCacheOptions>));
        }

        [Benchmark]
        public void Type_276_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<MemoryDistributedCacheOptions>));
        }


        [Benchmark]
        public void Type_277_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<MemoryDistributedCacheOptions>>));
        }

        [Benchmark]
        public void Type_277_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<MemoryDistributedCacheOptions>>));
        }

        [Benchmark]
        public void Type_277_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<MemoryDistributedCacheOptions>>));
        }


        [Benchmark]
        public void Type_278_BlueMilk()
        {
            var value = Providers[0]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<MemoryDistributedCacheOptions>>));
        }

        [Benchmark]
        public void Type_278_StructureMap()
        {
            var value = Providers[1]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<MemoryDistributedCacheOptions>>));
        }

        [Benchmark]
        public void Type_278_AspNetCore()
        {
            var value = Providers[2]
                .GetService(typeof(IEnumerable<IPostConfigureOptions<MemoryDistributedCacheOptions>>));
        }


        [Benchmark]
        public void Type_279_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<CacheTagHelperOptions>));
        }

        [Benchmark]
        public void Type_279_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<CacheTagHelperOptions>));
        }

        [Benchmark]
        public void Type_279_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<CacheTagHelperOptions>));
        }


        [Benchmark]
        public void Type_280_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<CacheTagHelperOptions>));
        }

        [Benchmark]
        public void Type_280_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<CacheTagHelperOptions>));
        }

        [Benchmark]
        public void Type_280_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<CacheTagHelperOptions>));
        }


        [Benchmark]
        public void Type_281_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<CacheTagHelperOptions>>));
        }

        [Benchmark]
        public void Type_281_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<CacheTagHelperOptions>>));
        }

        [Benchmark]
        public void Type_281_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<CacheTagHelperOptions>>));
        }


        [Benchmark]
        public void Type_282_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<CacheTagHelperOptions>>));
        }

        [Benchmark]
        public void Type_282_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<CacheTagHelperOptions>>));
        }

        [Benchmark]
        public void Type_282_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<CacheTagHelperOptions>>));
        }


        [Benchmark]
        public void Type_283_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILogger<JsonResultExecutor>));
        }

        [Benchmark]
        public void Type_283_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILogger<JsonResultExecutor>));
        }

        [Benchmark]
        public void Type_283_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILogger<JsonResultExecutor>));
        }


        [Benchmark]
        public void Type_284_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<CorsOptions>));
        }

        [Benchmark]
        public void Type_284_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<CorsOptions>));
        }

        [Benchmark]
        public void Type_284_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<CorsOptions>));
        }


        [Benchmark]
        public void Type_285_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<CorsOptions>));
        }

        [Benchmark]
        public void Type_285_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<CorsOptions>));
        }

        [Benchmark]
        public void Type_285_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<CorsOptions>));
        }


        [Benchmark]
        public void Type_286_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<CorsOptions>>));
        }

        [Benchmark]
        public void Type_286_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<CorsOptions>>));
        }

        [Benchmark]
        public void Type_286_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<CorsOptions>>));
        }


        [Benchmark]
        public void Type_287_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<CorsOptions>>));
        }

        [Benchmark]
        public void Type_287_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<CorsOptions>>));
        }

        [Benchmark]
        public void Type_287_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<CorsOptions>>));
        }


        [Benchmark]
        public void Type_288_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_288_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_288_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<LoggerFilterOptions>));
        }


        [Benchmark]
        public void Type_289_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IOptionsChangeTokenSource<LoggerFilterOptions>>));
        }

        [Benchmark]
        public void Type_289_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IOptionsChangeTokenSource<LoggerFilterOptions>>));
        }

        [Benchmark]
        public void Type_289_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IOptionsChangeTokenSource<LoggerFilterOptions>>));
        }


        [Benchmark]
        public void Type_290_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsMonitorCache<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_290_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsMonitorCache<LoggerFilterOptions>));
        }

        [Benchmark]
        public void Type_290_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsMonitorCache<LoggerFilterOptions>));
        }


        [Benchmark]
        public void Type_291_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<LoggerFilterOptions>>));
        }

        [Benchmark]
        public void Type_291_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<LoggerFilterOptions>>));
        }

        [Benchmark]
        public void Type_291_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<LoggerFilterOptions>>));
        }


        [Benchmark]
        public void Type_292_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<LoggerFilterOptions>>));
        }

        [Benchmark]
        public void Type_292_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<LoggerFilterOptions>>));
        }

        [Benchmark]
        public void Type_292_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<LoggerFilterOptions>>));
        }


        [Benchmark]
        public void Type_293_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptions<WebEncoderOptions>));
        }

        [Benchmark]
        public void Type_293_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptions<WebEncoderOptions>));
        }

        [Benchmark]
        public void Type_293_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptions<WebEncoderOptions>));
        }


        [Benchmark]
        public void Type_294_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IOptionsFactory<WebEncoderOptions>));
        }

        [Benchmark]
        public void Type_294_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IOptionsFactory<WebEncoderOptions>));
        }

        [Benchmark]
        public void Type_294_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IOptionsFactory<WebEncoderOptions>));
        }


        [Benchmark]
        public void Type_295_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IConfigureOptions<WebEncoderOptions>>));
        }

        [Benchmark]
        public void Type_295_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IConfigureOptions<WebEncoderOptions>>));
        }

        [Benchmark]
        public void Type_295_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IConfigureOptions<WebEncoderOptions>>));
        }


        [Benchmark]
        public void Type_296_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IPostConfigureOptions<WebEncoderOptions>>));
        }

        [Benchmark]
        public void Type_296_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IPostConfigureOptions<WebEncoderOptions>>));
        }

        [Benchmark]
        public void Type_296_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IPostConfigureOptions<WebEncoderOptions>>));
        }


        [Benchmark]
        public void Type_297_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(IEnumerable<IStartupFilter>));
        }

        [Benchmark]
        public void Type_297_StructureMap()
        {
            var value = Providers[1].GetService(typeof(IEnumerable<IStartupFilter>));
        }

        [Benchmark]
        public void Type_297_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(IEnumerable<IStartupFilter>));
        }


        [Benchmark]
        public void Type_298_BlueMilk()
        {
            var value = Providers[0].GetService(typeof(ILogger<Microsoft.AspNetCore.Hosting.Internal.WebHost>));
        }

        [Benchmark]
        public void Type_298_StructureMap()
        {
            var value = Providers[1].GetService(typeof(ILogger<Microsoft.AspNetCore.Hosting.Internal.WebHost>));
        }

        [Benchmark]
        public void Type_298_AspNetCore()
        {
            var value = Providers[2].GetService(typeof(ILogger<Microsoft.AspNetCore.Hosting.Internal.WebHost>));
        }
        */
}