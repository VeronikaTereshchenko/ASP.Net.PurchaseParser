using AngleSharp.Html.Dom;


namespace Parser._ASP.Net.Models
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
