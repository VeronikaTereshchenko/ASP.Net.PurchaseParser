using AngleSharp.Html.Dom;


namespace Parser._ASP.Net.Parsers.Interfaces
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
