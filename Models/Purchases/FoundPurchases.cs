namespace Parser._ASP.Net.Models.Purchases
{
    public class FoundPurchases
    {
        public string PurchaseName { get; set; } = string.Empty;

        public string PagesPeriod { get; set; } = string.Empty;

        public int PurchasesListCount { get; set; } = 0;

        public List<Card> PurchasesList { get; set; } = new List<Card>();
    }
}
