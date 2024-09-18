using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Controllers.Parsers.Exceptions;
using Parser._ASP.Net.Parsers.Interfaces;

namespace Parser._ASP.Net.Controllers.Parsers
{
    [NonController]
    static class HtmlLoader
    {
        private static SocketsHttpHandler socketHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        private static HttpClient _httpClient = new HttpClient(socketHandler);
        static HtmlLoader()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public static async Task<string> GetPageByPageNumAndPurchase(int num, IParserSettings purchaseSettings)
        {
            //кодируем словj, по которому идёт выборка закупок
            //encode the name by which the purchases are selected
            var decodeName = HttpUtility.UrlEncode(purchaseSettings.PurchaseName);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = purchaseSettings.BaseUrl.Replace("{purchaseName}", decodeName).Replace("{number}", num.ToString());

            var response = await _httpClient.GetAsync(currentUrl, HttpCompletionOption.ResponseHeadersRead);
            //the error about not accessing the page is caught in the PurchaseController.cs
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
