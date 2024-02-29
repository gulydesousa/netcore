using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    /// <summary>
    /// Service class for searching stocks using Finnhub API.
    /// </summary>
    public class FinnhubSearchStocksService : IFinnhubSearchStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinnhubSearchStocksService"/> class.
        /// </summary>
        /// <param name="finnhubRepository">The Finnhub repository.</param>
        public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        /// <summary>
        /// Searches stocks using the specified stock symbol.
        /// </summary>
        /// <param name="stockSymbolToSearch">The stock symbol to search.</param>
        /// <returns>A dictionary containing the search results.</returns>
        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            try
            {
                //invoke repository
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

                //return response dictionary back to the caller
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
dotnet user-secrets init --project StocksApp.UI
dotnet user-secrets set "FinnhubToken" "cc676uaad3i9rj8tb1s0" --project StocksApp.UI
*/