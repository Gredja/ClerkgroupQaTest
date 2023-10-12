using MongoDB.Driver;

namespace Service;

public class Startup
{
    public void ConfigureServices(IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services.AddControllers();
        services.AddScoped<DB>();
        services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IConfiguration>();
            var settings = MongoClientSettings.FromConnectionString(options["Connection"]);

            return new MongoClient(settings);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting()
            .UseEndpoints(endpoints => endpoints
                .MapControllers());
    }
}