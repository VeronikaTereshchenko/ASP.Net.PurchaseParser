using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Parser._ASP.Net.Models.Purchases;
using System.Text.RegularExpressions;

namespace Parser._ASP.Net.Parsers
{
    public abstract class HtmlSelectorParser
    {
        public abstract List<Card> Parse(IHtmlDocument document);

        public string GetTextContent(IElement cardHtml, string selector)
        {
            if (cardHtml == null)
                return " information is not found";

            var textHtml = cardHtml.QuerySelector(selector);

            if (textHtml == null)
                return " information is not found";

            var textContent = textHtml.TextContent;
            textContent = Regex.Replace(textContent, @"[\r\n\t]", " ");
            textContent = Regex.Replace(textContent, @"\s+", " ");

            return textContent;
        }

        protected decimal GetDecimalNum(string valuseStr)
        {
            var resultString = string.Join(string.Empty, Regex.Matches(valuseStr, @"\d+\,?").OfType<Match>().Select(m => m.Value));

            if (decimal.TryParse(resultString, out var decimalNum))
                return decimalNum;

            return 0;
        }
    }
}
