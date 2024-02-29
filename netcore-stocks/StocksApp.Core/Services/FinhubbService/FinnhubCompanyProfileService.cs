using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    /// <summary>
    /// Service class for retrieving company profile from Finnhub.
    /// </summary>
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinnhubCompanyProfileService"/> class.
        /// </summary>
        /// <param name="finnhubRepository">The Finnhub repository.</param>
        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        /// <summary>
        /// Retrieves the company profile for the specified stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <returns>The company profile as a dictionary.</returns>
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                // Invoke repository
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
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