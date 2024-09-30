using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Controllers.Parsers.Exceptions;
using Parser._ASP.Net.Models;

namespace Parser._ASP.Net.Controllers.Parsers
{
    [NonController]
    class ParserWorker<T> where T : class
    {
        private IParser<T> _parser;
        private IParserSettings _parserSettings;
        public event Action<T> OnNewData;
        public event Action OnCompleted;

        public IParser<T> Parser
        {
            get { return _parser; }
            set { _parser = value; }
        }

        public IParserSettings ParserSettings
        {
            get { return _parserSettings; }
            set { _parserSettings = value; }
        }

        public ParserWorker(IParser<T> parser) => _parser = parser;

        public ParserWorker(IParser<T> parser, IParserSettings settings) : this(parser) => _parserSettings = settings;

        public async Task Start() 
        {
            await Worker();
        }

        async Task Worker()
        {
            //проходимся по номерам страниц
            //walk through the page numbers
            for (int pageNum = _parserSettings.FirstPageNum; pageNum <= _parserSettings.LastPageNum; pageNum++)
            {
                var source = await HtmlLoader.GetPageByPageNumAndPurchase(pageNum, _parserSettings);
                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);
                //достаём инф. из каждой карточки на странице по тегам и классам
                //retrieve information from each card on the page by tags and classes
                var parsedInfo = _parser.Parse(document);

                //передаём извлечённую инф. из карточек в метод Main
                // pass the extracted information from cards to the Main method
                OnNewData?.Invoke(parsedInfo);
            }

            OnCompleted?.Invoke();
        }
    }
}
