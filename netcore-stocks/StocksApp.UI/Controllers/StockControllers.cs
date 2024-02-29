using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.FinnhubService;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;

        private readonly IFinnhubPriceQuoteService _finnhubPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
        private readonly IFinnhubStocksService _finnhubStocksService;

        private readonly ILogger<StocksController> _logger;


        /// <summary>
        /// Constructor for StocksController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradingOptions config through Options pattern</param>
        /// <param name="finnhubPriceQuoteService">Injecting FinnhubPriceQuoteService</param>
        /// <param name="finnhubCompanyProfileService">Injecting FinnhubCompanyProfileService</param>
        /// <param name="finnhubSearchStocksService">Injecting FinnhubSearchStocksService</param>
        /// <param name="finnhubStocksService">Injecting FinnhubStocksService</param>
        /// <param name="logger">Injecting ILogger</param>
        public StocksController(IOptions<TradingOptions> tradingOptions
            , IFinnhubPriceQuoteService finnhubPriceQuoteService
            , IFinnhubCompanyProfileService finnhubCompanyProfileService
            , IFinnhubSearchStocksService finnhubSearchStocksService
            , IFinnhubStocksService finnhubStocksService
            , ILogger<StocksController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubPriceQuoteService = finnhubPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubSearchStocksService = finnhubSearchStocksService;
            _finnhubStocksService = finnhubStocksService;
            _logger = logger;
        }


        /// <summary>
        /// Explore method for retrieving and filtering stocks.
        /// </summary>
        /// <param name="stock">The stock symbol to explore.</param>
        /// <param name="showAll">Flag indicating whether to show all stocks or filter based on popular stocks.</param>
        /// <returns>The IActionResult representing the view of the stocks.</returns>
        [Route("/")]
        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            //logger
            _logger.LogInformation("In StocksController.Explore() action method");
            _logger.LogDebug("stock: {stock}, showAll: {showAll}", stock, showAll);

            //get company profile from API server
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubStocksService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary is not null)
            {
                //filter stocks
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (Top25PopularStocksList is not null)
                    {
                        stocksDictionary = stocksDictionary
                         .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }

                //convert dictionary objects into Stock objects
                stocks = stocksDictionary
                 .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();
            }

            ViewBag.stock = stock;
            return View(stocks);
        }
    }
}