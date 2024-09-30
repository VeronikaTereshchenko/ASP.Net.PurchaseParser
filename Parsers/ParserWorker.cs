using AngleSharp.Html.Parser;
using Microsoft.Extensions.Options;
using Parser._ASP.Net.Models.Purchases;
using Parser._ASP.Net.Parsers.Purchases;

namespace Parser._ASP.Net.Controllers.Parsers
{
    public class ParserWorker
    {
        public PurchaseParser _parser;
        private HtmlLoader _htmlLoader;
        private PurchaseSettings _purchaseSettings;

        public ParserWorker(IOptions<PurchaseSettings> purchaseOption, PurchaseParser parser, HtmlLoader htmlLoader)
        {
            _purchaseSettings = purchaseOption.Value;
            _htmlLoader = htmlLoader;
            _parser = parser;
        }

        public async Task<List<Card>> GetProductsAsync()
        {
            var parsedInfo = new List<Card>();

            for (int pageNum = _purchaseSettings.FirstPageNum; pageNum <= _purchaseSettings.LastPageNum; pageNum++)
            {
                var source = await _htmlLoader.GetPageAsync(pageNum);

                if (string.IsNullOrEmpty(source))
                    continue;

                var htmlParser = new HtmlParser();
                var document = await htmlParser.ParseDocumentAsync(source);

                if (document == null)
                    continue;

                //достаём инф. из каждой карточки на странице по тегам и классам
                //retrieve information from each card on the page by tags and classes
                var result = _parser.Parse(document);

                parsedInfo.AddRange(result);
            }

            return parsedInfo;
        }
    }
}
