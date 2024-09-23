using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Models.Purchases;
using Parser._ASP.Net.Parsers.Interfaces;
using System.Text.RegularExpressions;


namespace Parser._ASP.Net.Parsers.Purchases
{
    public class PurchaseParser : HtmlSelectorParser
    {
        public override List<Card> Parse(IHtmlDocument document)
        {
            //ищем карточки (карточка хранит инф. об одном объекте, имя объекта задаётся в appsettings.json)
            //search for a card
            var purchaseCardsHtml = document.QuerySelectorAll("div.row.no-gutters.registry-entry__form.mr-0");

            var cards = new List<Card>();

            if (purchaseCardsHtml == null)
            {
                return cards;
            }

            foreach (var purchaseCardHtml in purchaseCardsHtml)
            {
                var card = new Card()
                {
                    Law = GetTextContent(purchaseCardHtml, "div.col-9.p-0.registry-entry__header-top__title.text-truncate"),
                    Number = GetTextContent(purchaseCardHtml, "div.registry-entry__header-mid__number a"),
                    PurchaseObject = GetTextContent(purchaseCardHtml, "div.registry-entry__body-value"),
                    Organization = GetTextContent(purchaseCardHtml, "div.registry-entry__body-href"),
                    StartPrice = GetDecimalNum(GetTextContent(purchaseCardHtml, "div.price-block__value"))
                };

                cards.Add(card);
            }

            return cards;
        }
    }

}

