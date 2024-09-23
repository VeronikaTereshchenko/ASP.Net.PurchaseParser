using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Parsers.Interfaces;

namespace Parser._ASP.Net.Models.Purchases
{
    public class PurchaseSettings : IParserSettings
    {
        public string BaseUrl { get; set; }

        public string PurchaseName { get; set; }

        public int FirstPageNum { get; set; }

        public int LastPageNum { get; set; }
    }
}
