using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Models.Purchases;
using Parser._ASP.Net.Parsers.Purchases;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.Configure<PurchaseSettings>(
            builder.Configuration.GetSection(PurchaseSettings.PurchaseSection));
        builder.Services.AddScoped<PurchaseParser>();
        builder.Services.AddScoped<ParserWorker>();
        builder.Services.AddSingleton<HtmlLoader>();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}