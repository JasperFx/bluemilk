using BlueMilk.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1 {
    public class Program {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseBlueMilk()
                .UseStartup<Startup>()
                .Build();

        public static void Main(string[] args) {
            BuildWebHost(args).Run();
        }
    }
}
