using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.ConfigurationManager;
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

        static HtmlLoader() => _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

        public static async Task<string> GetPageAsync(int num)
        {
            var settings = Configurations.GetPurchaseSettings;

            //кодируем словj, по которому идёт выборка закупок
            //encode the name by which the purchases are selected
            var encodeName = HttpUtility.UrlEncode(settings.PurchaseName);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = settings.BaseUrl.Replace("{PHRASE}", encodeName).Replace("{NUMBER}", num.ToString());

            var response = await _httpClient.GetAsync(currentUrl);

            //the error about not accessing the page is caught in the PurchaseController.cs
            if(response != null && response.StatusCode == HttpStatusCode.OK) 
            {
                return await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine($"link couuldn't be accessed: {settings.BaseUrl}");
            return string.Empty;
        }
    }
}
