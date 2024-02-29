using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts.FinnhubService;
using StockMarketSolution;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;

namespace Tests.ControllerTests
{
    public class StocksControllerTest
    {
        private readonly IFinnhubPriceQuoteService _finnhubPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
        private readonly IFinnhubStocksService _finnhubStocksService;

        private readonly Mock<IFinnhubPriceQuoteService> _finnhubPriceQuoteServiceMock;
        private readonly Mock<IFinnhubCompanyProfileService> _finnhubCompanyProfileServiceMock;
        private readonly Mock<IFinnhubSearchStocksService> _finnhubSearchStocksServiceMock;
        private readonly Mock<IFinnhubStocksService> _finnhubStocksServiceMock;

        private readonly Fixture _fixture;
        private readonly ILogger<StocksController> _logger;

        public StocksControllerTest()
        {
            _fixture = new Fixture();

            _finnhubPriceQuoteServiceMock = new Mock<IFinnhubPriceQuoteService>();
            _finnhubCompanyProfileServiceMock = new Mock<IFinnhubCompanyProfileService>();
            _finnhubSearchStocksServiceMock = new Mock<IFinnhubSearchStocksService>();
            _finnhubStocksServiceMock = new Mock<IFinnhubStocksService>();


            _logger = new Mock<ILogger<StocksController>>().Object;

            _finnhubPriceQuoteService = _finnhubPriceQuoteServiceMock.Object;
            _finnhubCompanyProfileService = _finnhubCompanyProfileServiceMock.Object;
            _finnhubSearchStocksService = _finnhubSearchStocksServiceMock.Object;
            _finnhubStocksService = _finnhubStocksServiceMock.Object;
        }


        #region Index

        [Fact]
        public async Task Explore_StockIsNull_ShouldReturnExploreViewWithStocksList()
        {
            //Arrange
            IOptions<TradingOptions> tradingOptions = Options.Create(new TradingOptions() { DefaultOrderQuantity = 100, Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO" });

            StocksController stocksController = new StocksController(tradingOptions, _finnhubPriceQuoteService, _finnhubCompanyProfileService, _finnhubSearchStocksService, _finnhubStocksService, _logger);

            List<Dictionary<string, string>>? stocksDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");

            _finnhubStocksServiceMock
             .Setup(temp => temp.GetStocks())
             .ReturnsAsync(stocksDict);

            var expectedStocks = stocksDict!
              .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
             .ToList();

            //Act
            IActionResult result = await stocksController.Explore(null, true);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(expectedStocks);
        }
        #endregion


    }
}