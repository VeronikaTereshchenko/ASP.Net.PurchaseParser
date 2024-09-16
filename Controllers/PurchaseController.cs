using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Models;
using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Controllers.Parsers.Purchases;

namespace Parser._ASP.Net.Controllers
{
    public class PurchaseController : Controller
    {
        [HttpGet]
        public async Task ParsePurchases(string searchString, int pageNumber)
        {
            var response = Response;
            response.Headers.ContentLanguage = "ru-Ru";
            response.Headers.ContentType = "text/plain; charset=utf-8";

            var parsedPurchaseList = new List<List<Card>>();
            var parser = new ParserWorker<List<Card>>(new Purchase_Parser());
            IParserSettings settings = new PurchaseSettings("бумага", 1, 5);
            parser.ParserSettings = settings;

            parser.OnNewData += Parser_OnNewData;
            parser.OnCompleted += Parser_OnComplete;

            try
            {
                await parser.Start();
            }
            catch (HttpRequestException ex)
            {
                HttpContext.Response.WriteAsync(ex.Message);
            }

            void Parser_OnNewData(List<Card> arg2)
            {
                //добавление данных с карточек на одной странице
                //add data from cards on one page
                parsedPurchaseList.Add(arg2);
            }

            void Parser_OnComplete()
            {
                //поиск по страницам завершён
                // page search complete
                HttpContext.Response.WriteAsync("All works done!!!\n");
            }

            PrintParsedPurchases(parsedPurchaseList);
        }

        private void PrintParsedPurchases(List<List<Card>> parsedPurchaseList)
        {
            if (parsedPurchaseList != null)
            {
                foreach (List<Card> list in parsedPurchaseList)
                {
                    foreach (Card card in list)
                    {
                        HttpContext.Response.WriteAsync("Card\n");

                        foreach (var prop in typeof(Card).GetProperties())
                            HttpContext.Response.WriteAsync($"{prop.Name}: {prop.GetValue(card)}\n");

                        HttpContext.Response.WriteAsync("\n\n");
                    }
                }
            }
        }
    }
}
