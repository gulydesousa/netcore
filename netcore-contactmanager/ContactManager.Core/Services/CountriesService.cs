using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private fields
        private readonly ICountriesRepository _countriesRepository;
        const string worksheetName = "Countries";

        /// <summary>
        /// Converts a <see cref="Country"/> object to a <see cref="CountryResponse"/> object.
        /// </summary>
        /// <param name="country">The <see cref="Country"/> object to convert.</param>
        /// <returns>The converted <see cref="CountryResponse"/> object.</returns>
        private CountryResponse? ToResponse(Country country)
        {
            return country?.ToCountryResponse();
        }

        /// <summary>
        /// Checks if a country with the specified name exists.
        /// </summary>
        /// <param name="countryName">The name of the country to check.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a boolean value indicating whether the country exists.</returns>
        private async Task<bool> CountryExists(string countryName)
        {
            return await _countriesRepository.GetCountryByName(countryName) != null;
        }

        //Constructor
        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        /// <summary>
        /// Gets a list of all countries.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a list of <see cref="CountryResponse"/> objects.</returns>
        public async Task<IList<CountryResponse>> GetCountries()
        {
            List<Country> countries = await _countriesRepository.GetCountries().ConfigureAwait(false);
            return countries.Select(c => c.ToCountryResponse()).ToList();
        }

        /// <summary>
        /// Gets a country by its ID.
        /// </summary>
        /// <param name="countryID">The ID of the country to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the <see cref="CountryResponse"/> object if found, or null if not found.</returns>
        public async Task<CountryResponse?> GetCountryById(Guid? countryID)
        {
            if (countryID == null) throw new ArgumentNullException(nameof(countryID));

            //When CountryId is valid, it should return the country
            Country? country = await _countriesRepository.GetCountryById(countryID);

            if (country == null)
            {
                throw new ArgumentNullException(nameof(countryID));
            }

            return ToResponse(country);
        }

        /// <summary>
        /// Adds a new country.
        /// </summary>
        /// <param name="countryAddRequest">The <see cref="CountryAddRequest"/> object containing the details of the country to add.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the added <see cref="CountryResponse"/> object.</returns>
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {

            //Check if "countryAddRequest" is not null and "countryAddRequest.CountryName" is not null. 
            if (countryAddRequest == null || countryAddRequest.CountryName == null)
            {
                throw new ArgumentException("The country add request or the country name provided in the request is null. Please provide a valid request.");
            }

            //Check if "countryAddRequest.CountryName" is duplicated.
            if (await CountryExists(countryAddRequest.CountryName))
            {
                throw new ArgumentException($"A country with the name '{countryAddRequest.CountryName}' already exists. Please provide a unique country name.");
            }


            //Convert "countryAddRequest" from "CountryAddRequest" type to "Country". 
            Country country = countryAddRequest.ToCountry();

            //Generate a new CountrylD 
            country.Id = Guid.NewGuid();

            //Then add it into List<Country> 
            await _countriesRepository.AddCountry(country);

            //Return CountryResponse object with generated CountrylD
            return ToResponse(country)!;
        }

        /// <summary>
        /// Uploads countries from an Excel file.
        /// </summary>
        /// <param name="formFile">The <see cref="IFormFile"/> representing the Excel file to upload.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the number of countries uploaded.</returns>
        public async Task<int> UploadCountryFromExcelFile(IFormFile formFile)
        {
            int result = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);

                using (ExcelPackage excel = new ExcelPackage(ms))
                {
                    if (excel.Workbook.Worksheets.Count == 0) throw new InvalidOperationException("Excel file doesn't have any worksheets");
                    if (!excel.Workbook.Worksheets.Any(ws => ws.Name == worksheetName))
                    {
                        throw new InvalidOperationException($"Excel file doesn't have a worksheet with the name '{worksheetName}'");
                    }

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets[worksheetName];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int i = 2; i <= rowCount; i++)
                    {
                        string? countryName = worksheet.Cells[i, 1].Value?.ToString();

                        if (!string.IsNullOrEmpty(countryName) && !await CountryExists(countryName))
                        {
                            Country request = new Country
                            {
                                CountryName = countryName
                            };

                            await _countriesRepository.AddCountry(request);
                            result++;
                        }
                    }
                }
            }
            return result;
        }
    }
}