using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Parser._ASP.Net.Models.Purchases;

namespace Parser._ASP.Net.Parsers
{
    public abstract class HtmlSelectorParser
    {
        public abstract List<Card> Parse(IHtmlDocument document);
    }
}
