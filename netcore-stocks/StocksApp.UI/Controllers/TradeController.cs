using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts.DTO;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using StockMarketSolution.Filters;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;

        private readonly ISellOrdersService _stocksSellService;
        private readonly IBuyOrdersService _stocksBuyService;

        private readonly IFinnhubPriceQuoteService _finnhubPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
        private readonly IFinnhubStocksService _finnhubStocksService;

        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;


        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// <param name="configuration">Injecting IConfiguration</param>
        public TradeController(IOptions<TradingOptions> tradingOptions,
            ISellOrdersService stocksSellService,
            IBuyOrdersService stocksBuyService
            , IFinnhubPriceQuoteService finnhubPriceQuoteService
            , IFinnhubCompanyProfileService finnhubCompanyProfileService
            , IFinnhubSearchStocksService finnhubSearchStocksService
            , IFinnhubStocksService finnhubStocksService
            , IConfiguration configuration
            , ILogger<TradeController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksBuyService = stocksBuyService;
            _stocksSellService = stocksSellService;
            _finnhubPriceQuoteService = finnhubPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubSearchStocksService = finnhubSearchStocksService;
            _finnhubStocksService = finnhubStocksService;
            _configuration = configuration;
            _logger = logger;
        }
        [Route("~/[controller]/[action]/{stock?}")] //the action method should be the same as the action method name which in this case is Index
        [Route("[action]")] //the action method should be the same as the action method name which in this case is Index
        [Route("~/[controller]")] //specifies that the Index action can also be accessed using the URL path of the controller name, which is Trade
        public async Task<IActionResult> Index(string? stock)
        {
            //logger
            _logger.LogInformation("In TradeController.Index() action method");
            _logger.LogDebug("stockSymbol: {stockSymbol}", stock);


            //reset stock symbol if not exists
            if (!string.IsNullOrEmpty(stock))
                _tradingOptions.DefaultStockSymbol = stock;

            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";


            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubPriceQuoteService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade() { StockSymbol = _tradingOptions.DefaultStockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]), StockName = Convert.ToString(companyProfileDictionary["name"]), Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }


        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            //Todo esto se hace ahora desde el filtro CreateOrderActionFilters.cs
            /*
            //update date of order
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);


            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }
            */
            //invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksBuyService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }


        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            //Todo esto se hace ahora desde el filtro CreateOrderActionFilters.cs
            /*
            //update date of order
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }
            */

            //invoke service method
            SellOrderResponse sellOrderResponse = await _stocksSellService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }


        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            //invoke service methods
            List<BuyOrderResponse> buyOrderResponses = await _stocksBuyService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksSellService.GetSellOrders();

            //create model object
            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);
        }


        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            //Get list of orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stocksBuyService.GetBuyOrders());
            orders.AddRange(await _stocksSellService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradingOptions;

            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}