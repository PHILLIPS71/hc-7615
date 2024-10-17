using Bogus;
using HC_7572.Domain.Objects;
using Microsoft.EntityFrameworkCore;
using Location = HC_7572.Domain.Objects.Location;

namespace HC_7572.DbContexts;

public sealed class DatabaseSeeding : IHostedService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<DatabaseSeeding> _logger;

    public DatabaseSeeding(IServiceScopeFactory factory, ILogger<DatabaseSeeding> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _factory.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await database.Users.AnyAsync(cancellationToken))
            return;

        for (var x = 1; x <= 10; x++)
        {
            var faker = new Faker();
            var user = new User { Name = faker.Person.FullName };

            for (var y = 1; y <= 5; y++)
            {
                user.Locations.Add(new Location { Name = faker.Address.FullAddress() });
            }

            database.Users.Add(user);
        }

        await database.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("successfully seeded some fake data :)");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}