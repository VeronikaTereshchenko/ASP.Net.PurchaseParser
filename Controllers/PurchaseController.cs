using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Parsers.Purchases;
using Parser._ASP.Net.Parsers.Interfaces;
using Parser._ASP.Net.ConfigurationManager;
using Parser._ASP.Net.Models;
using Parser._ASP.Net.Models.Purchases;

namespace Parser._ASP.Net.Controllers
{
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        [Route("api/zakupki/search/get")]
        [HttpPost]
        public async Task<IActionResult> ParsePurchases() 
        {
            var parser = new ParserWorker(new PurchaseParser());

            List<Card> parsedPurchasesList = await parser.GetProductsAsync();

            var settings = Configurations.GetPurchaseSettings;

            var foundPurchases = new FoundPurchases()
            {
                PurchaseName = settings.PurchaseName,
                PagesPeriod = $"search through pages {settings.FirstPageNum} to {settings.LastPageNum}",
                PurchasesListCount = parsedPurchasesList.Count,
                PurchasesList = parsedPurchasesList
            };

            return Ok(foundPurchases);
        }
    }
}
