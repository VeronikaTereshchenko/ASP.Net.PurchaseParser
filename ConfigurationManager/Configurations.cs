using Parser._ASP.Net.Models.Purchases;

namespace Parser._ASP.Net.ConfigurationManager
{
    public static class Configurations
    {
        public static PurchaseSettings GetPurchaseSettings {get; set;} = new PurchaseSettings();

        public static void SetSettings(IConfiguration config)
        {
            config.GetSection("PurchaseSettings").Bind(GetPurchaseSettings);
        }
    }
}
