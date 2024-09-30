using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Models.Purchases;
using Microsoft.Extensions.Options;

namespace Parser._ASP.Net.Controllers
{
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseSettings _purchaseSettings;
        private ParserWorker _parser;

        public PurchaseController(IOptions<PurchaseSettings> purchaseOption, ParserWorker parserWorker) 
        {
            _purchaseSettings = purchaseOption.Value;
            _parser = parserWorker;
        }

        [Route("api/zakupki/get")]
        [HttpPost]
        public async Task<IActionResult> ParsePurchases() 
        {
            List<Card> parsedPurchasesList = await _parser.GetProductsAsync();

            var foundPurchases = new FoundPurchases()
            {
                PurchaseName = _purchaseSettings.PurchaseName,
                PagesPeriod = $"search through pages {_purchaseSettings.FirstPageNum} to {_purchaseSettings.LastPageNum}",
                PurchasesListCount = parsedPurchasesList.Count,
                PurchasesList = parsedPurchasesList
            };

            return Ok(foundPurchases);
        }
    }
}
