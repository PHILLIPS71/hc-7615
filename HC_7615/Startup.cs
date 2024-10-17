using HC_7572.DbContexts;
using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace HC_7572;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        _configuration = configuration;
        _environment = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(_configuration.GetConnectionString("DatabaseConnection"));
            });

        services
            .AddHostedService<MigratorHostedService<ApplicationDbContext>>()
            .AddHostedService<DatabaseSeeding>();

        services
            .AddGraphQLServer()
            .ModifyOptions(opt => opt.StrictValidation = false)
            .AddHC_7572Types()
            .AddProjections()
            .AddPagingArguments()
            .AddFiltering()
            .AddSorting();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (!_environment.IsDevelopment())
            app.UseHttpsRedirection();

        app
            .UseRouting()
            .UseWebSockets()
            .UseEndpoints(endpoint =>
                endpoint
                    .MapGraphQL()
                    .WithOptions(new GraphQLServerOptions
                    {
                        Tool = { Enable = _environment.IsDevelopment() }
                    })
            );
    }
}