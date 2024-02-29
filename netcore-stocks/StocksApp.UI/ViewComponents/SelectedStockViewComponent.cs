using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Services.FinnhubService;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly ISellOrdersService _stocksService;

        private readonly IFinnhubPriceQuoteService _finnhubPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
        private readonly IFinnhubStocksService _finnhubStocksService;

        private readonly IConfiguration _configuration;


        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// <param name="configuration">Injecting IConfiguration</param>
        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions
            , ISellOrdersService stocksService
            , IFinnhubPriceQuoteService finnhubPriceQuoteService
            , IFinnhubCompanyProfileService finnhubCompanyProfileService
            , IFinnhubSearchStocksService finnhubSearchStocksService
            , IFinnhubStocksService finnhubStocksService
            , IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksService = stocksService;
            _finnhubPriceQuoteService = finnhubPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubSearchStocksService = finnhubSearchStocksService;
            _finnhubStocksService = finnhubStocksService;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stockSymbol != null)
            {
                companyProfileDict = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubPriceQuoteService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }
    }
}