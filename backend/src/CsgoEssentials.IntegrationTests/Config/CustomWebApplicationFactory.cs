using CsgoEssentials.Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CsgoEssentials.IntegrationTests.Config
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DataContext>));

                services.Remove(descriptor);

                var serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });


                //using (var scope = serviceProvider.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var context = scopedServices.GetRequiredService<DataContext>();
                //    var logger = scopedServices
                //        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                //    context.Database.EnsureCreated();

                //    try
                //    {
                //        //DummyData.GenerateUsers(context, true).Wait();
                //    }
                //    catch (Exception ex)
                //    {
                //        logger.LogError(ex, "An error occurred seeding the " +
                //            "database with test messages. Error: {Message}", ex.Message);
                //    }
                //}
            });
        }

        public DataContext GetContext()
        {
            var factory = new CustomWebApplicationFactory<TStartup>();
            var scope = factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<DataContext>();
        }
    }
}
