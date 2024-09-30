using Microsoft.AspNetCore.Mvc;

namespace Parser._ASP.Net.Controllers.Parsers.Exceptions
{
    [NonController]
    internal class HttpRequestError : SystemException
    {
        public HttpRequestError(string message) : base(message) { }
    }
}
