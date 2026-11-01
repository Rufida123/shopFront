using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopfront.Infrastructure.Persistence;

namespace Shopfront.IntegrationTests;

public sealed class ShopfrontApiFactory : WebApplicationFactory<Program>
{
    private const string TestingEnvironment = "Testing";

    private readonly string databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(TestingEnvironment);

        builder.ConfigureTestServices(services =>
        {
            var registrations = services.Where(IsDbContextRegistration).ToList();
            foreach (var registration in registrations)
            {
                services.Remove(registration);
            }

            services.AddDbContext<ShopfrontDbContext>(options => options.UseInMemoryDatabase(databaseName));
        });
    }

    private static bool IsDbContextRegistration(ServiceDescriptor descriptor) =>
        descriptor.ServiceType == typeof(DbContextOptions<ShopfrontDbContext>)
        || descriptor.ServiceType.GenericTypeArguments.Contains(typeof(ShopfrontDbContext));
}
