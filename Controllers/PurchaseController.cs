using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Parsers.Purchases;
using Parser._ASP.Net.Parsers.Interfaces;

namespace Parser._ASP.Net.Controllers
{
    public class PurchaseController : Controller
    {
        [Route("")]
        [HttpGet]
        public  async Task ParsePurchases() 
        {
            Response.Headers.ContentLanguage = "ru-Ru";
            Response.Headers.ContentType = "text/plain; charset=utf-8";

            var parsedPurchaseList = new List<List<Card>>();
            var parser = new ParserWorker<List<Card>>(new Purchase_Parser());
            
            IParserSettings settings = new PurchaseSettings("труба", 1, 1);
            parser.ParserSettings = settings;

            parser.OnNewData += Parser_OnNewData;
            parser.OnCompleted += Parser_OnComplete;

            try
            {
                var r = Request;
                await parser.Start();
            }
            catch (HttpRequestException ex)
            {
                Response.WriteAsync(ex.Message);
            }

            void Parser_OnNewData(List<Card> cards)
            {
                //добавление данных с карточек на одной странице
                //add data from cards on one page
                if(cards.Count > 0)
                    parsedPurchaseList.Add(cards);
            }

            void Parser_OnComplete()
            {
                //поиск по страницам завершён
                // page search complete
                Response.WriteAsync("All works done!!!\n\n");
            }

            PrintParsedPurchases(parsedPurchaseList, Response);
        }

        private static void PrintParsedPurchases(List<List<Card>> parsedpurchaselist, HttpResponse response)
        {
            if (parsedpurchaselist.Count > 0)
            {
                foreach (List<Card> list in parsedpurchaselist)
                {
                    foreach (Card card in list)
                    {
                        response.WriteAsync("card\n");

                        foreach (var prop in typeof(Card).GetProperties())
                            response.WriteAsync($"{prop.Name}: {prop.GetValue(card)}\n");

                        response.WriteAsync("\n\n");
                    }
                }
            }

            else
            {
                response.WriteAsync("no purchase information is available at the specified url\n");
            }
        }
    }
}
