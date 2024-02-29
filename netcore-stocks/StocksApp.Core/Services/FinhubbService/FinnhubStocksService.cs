using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    /// <summary>
    /// Service class for interacting with Finnhub stocks.
    /// </summary>
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinnhubStocksService"/> class.
        /// </summary>
        /// <param name="finnhubRepository">The Finnhub repository.</param>
        public FinnhubStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        /// <summary>
        /// Retrieves the list of stocks.
        /// </summary>
        /// <returns>The list of stocks as a collection of dictionaries.</returns>
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                //invoke repository
                List<Dictionary<string, string>>? responseDictionary = await _finnhubRepository.GetStocks();

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
dotnet user-secrets init --project StockMarketSolution
dotnet user-secrets set "FinnhubToken" "cc676uaad3i9rj8tb1s0" --project StockMarketSolution
*/