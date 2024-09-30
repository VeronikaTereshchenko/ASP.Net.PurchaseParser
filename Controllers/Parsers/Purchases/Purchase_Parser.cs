using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Models;
using System.Text.RegularExpressions;


namespace Parser._ASP.Net.Controllers.Parsers.Purchases
{
    [NonController]
    public class Purchase_Parser : IParser<List<Card>>
    {
        private Dictionary<string, System.Reflection.PropertyInfo> PropertyLookupByAttributeValueDictionary;
        private Dictionary<string, System.Reflection.PropertyInfo>.KeyCollection DictionaryKeys;

        public Purchase_Parser()
        {
            var cardProp = typeof(Card).GetProperties();

            PropertyLookupByAttributeValueDictionary = new Dictionary<string, System.Reflection.PropertyInfo>()
            { 
                //names of classes in the page code, by which we search for the required information
                //law
                { "col-9 p-0 registry-entry__header-top__title text-truncate", cardProp[0] },
                //number
                { "registry-entry__header-mid__number", cardProp[1] },
                //purchaseObject
                { "registry-entry__body-value", cardProp[2] },
                //Organization
                { "registry-entry__body-href", cardProp[3] },
                //StartPrice
                { "price-block__value", cardProp[4] }
            };

            DictionaryKeys = PropertyLookupByAttributeValueDictionary.Keys;
        }

        public List<Card> Parse(IHtmlDocument document)
        {
            //ищем карточку (карточка хранит инф. об одном объекте, полученному после поиска)
            //search for a card
            var purchaseCardsHtml = document.QuerySelectorAll("div").Where(item => item.ClassName != null
                                                                //из всего кода страницы выбираются только те классы, которые
                                                                //хранят карточки объектов
                                                                //from the whole page code only those classes are selected that
                                                                //store cards 
                                                                && item.ClassName == "row no-gutters registry-entry__form mr-0");

            var cards = new List<Card>();

            foreach (var purchaseCardHtml in purchaseCardsHtml)
            {
                var card = new Card();

                foreach (var dicKey in DictionaryKeys)
                {
                    var cardInfo = purchaseCardHtml.GetElementsByClassName(dicKey);

                    if (cardInfo.Length > 0)
                    {
                        var info = cardInfo[0].TextContent;
                        info = Regex.Replace(info, @"[\r\n\t]", " ");
                        info = Regex.Replace(info, @"\s+", " ");
                        var cardProperty = PropertyLookupByAttributeValueDictionary[dicKey];
                        cardProperty.SetValue(card, info);
                    }
                }

                //If a card contains a filled price field, it means that it is not empty
                if (card.StartPrice != null)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }
    }
}

