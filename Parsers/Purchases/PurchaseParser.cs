using AngleSharp.Html.Dom;
using Parser._ASP.Net.Models.Purchases;


namespace Parser._ASP.Net.Parsers.Purchases
{
    public class PurchaseParser
    {
        public List<Card> Parse(IHtmlDocument document)
        {
            //ищем карточки (карточка хранит инф. об одном объекте, имя объекта задаётся в app_PurchaseSettings.json)
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
                    Law = purchaseCardHtml.GetTextContent( "div.col-9.p-0.registry-entry__header-top__title.text-truncate"),
                    Number = purchaseCardHtml.GetTextContent("div.registry-entry__header-mid__number a"),
                    PurchaseObject = purchaseCardHtml.GetTextContent("div.registry-entry__body-value"),
                    Organization = purchaseCardHtml.GetTextContent("div.registry-entry__body-href"),
                    StartPrice = purchaseCardHtml.GetDecimalNum("div.price-block__value")
                };

                cards.Add(card);
            }

            return cards;
        }
    }

}

