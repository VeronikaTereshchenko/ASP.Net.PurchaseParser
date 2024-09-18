using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Parsers.Interfaces;
using System.Text.RegularExpressions;


namespace Parser._ASP.Net.Parsers.Purchases
{
    [NonController]
    public class Purchase_Parser : IParser<List<Card>>
    {
        public List<Card> Parse(IHtmlDocument document)
        {
            //ищем карточку (карточка хранит инф. об одном объекте, полученному после поиска)
            //search for a card
            var purchaseCardsHtml = document.QuerySelectorAll("div.row.no-gutters.registry-entry__form.mr-0");

            var cards = new List<Card>();

            if(purchaseCardsHtml == null) 
            { 
                return cards;
            }

            foreach (var purchaseCardHtml in purchaseCardsHtml)
            {
                var card = new Card()
                {
                    Law = GetTextContent(purchaseCardHtml.QuerySelector("div.col-9.p-0.registry-entry__header-top__title.text-truncate")?.TextContent),
                    Number = GetTextContent(purchaseCardHtml.QuerySelector("div.registry-entry__header-mid__number a")?.TextContent),
                    PurchaseObject = GetTextContent(purchaseCardHtml.QuerySelector("div.registry-entry__body-value")?.TextContent),
                    Organization = GetTextContent(purchaseCardHtml.QuerySelector("div.registry-entry__body-href")?.TextContent),
                    StartPrice = GetTextContent(purchaseCardHtml.QuerySelector("div.price-block__value")?.TextContent)
                };

                cards.Add(card);
            }

            return cards;
        }

        private string GetTextContent(string textContent)
        {
            if(textContent != null)
            {
                textContent = Regex.Replace(textContent, @"[\r\n\t]", " ");
                return textContent = Regex.Replace(textContent, @"\s+", " ");
            }

            return " information is not found";
        }
    }
}

