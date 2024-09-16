internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        var app = builder.Build();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Purchase}/{action=ParsePurchases}"
            );

        app.Run();
    }
}