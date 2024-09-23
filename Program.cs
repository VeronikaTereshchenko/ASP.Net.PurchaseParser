using Parser._ASP.Net.ConfigurationManager;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        Configurations.SetSettings(config);

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}