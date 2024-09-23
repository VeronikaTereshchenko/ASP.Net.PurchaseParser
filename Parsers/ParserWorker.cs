using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.ConfigurationManager;
using Parser._ASP.Net.Models.Purchases;
using Parser._ASP.Net.Parsers;
using Parser._ASP.Net.Parsers.Interfaces;

namespace Parser._ASP.Net.Controllers.Parsers
{
    public class ParserWorker
    {
        public HtmlSelectorParser _parser;
        private IParserSettings _purchaseSettings = Configurations.GetPurchaseSettings;

        public ParserWorker(HtmlSelectorParser p) => _parser = p;

        public async Task<List<Card>> GetProductsAsync()
        {
            var parsedInfo = new List<Card>();

            for (int pageNum = _purchaseSettings.FirstPageNum; pageNum <= _purchaseSettings.LastPageNum; pageNum++)
            {
                var source = await HtmlLoader.GetPageAsync(pageNum);
                var htmlParser = new HtmlParser();
                var document = await htmlParser.ParseDocumentAsync(source);
                //достаём инф. из каждой карточки на странице по тегам и классам
                //retrieve information from each card on the page by tags and classes
                parsedInfo.AddRange(_parser.Parse(document));
            }

            return parsedInfo;
        }
    }
}
