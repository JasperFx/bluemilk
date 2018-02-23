using System.Collections.Generic;
using BlueMilk;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1 {
    public class Startup {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.Run(async context => {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        public void ConfigureContainer(ServiceRegistry services) {
            services.AddMvc();
            services.AddLogging();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());
        }
    }

    public class Config {
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource> {
            new ApiResource("api1", "My API")
        };

        public static IEnumerable<Client> GetClients() => new List<Client> {
            new Client {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets = {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = {
                    "api1"
                }
            }
        };
    }
}
