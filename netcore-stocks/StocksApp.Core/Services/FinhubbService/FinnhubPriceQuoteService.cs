using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    /// <summary>
    /// Service class for retrieving stock price quotes from Finnhub.
    /// </summary>
    public class FinnhubPriceQuoteService : IFinnhubPriceQuoteService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinnhubPriceQuoteService"/> class.
        /// </summary>
        /// <param name="finnhubRepository">The Finnhub repository.</param>
        public FinnhubPriceQuoteService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        /// <summary>
        /// Retrieves the stock price quote for the specified stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <returns>The stock price quote as a dictionary of key-value pairs.</returns>
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                // Invoke repository
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

                // Return response dictionary back to the caller
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
    }
}
/*
User Secrets:
dotnet user-secrets init --project StockMarketSolution
dotnet user-secrets set "FinnhubToken" "cc676uaad3i9rj8tb1s0" --project StockMarketSolution
*/