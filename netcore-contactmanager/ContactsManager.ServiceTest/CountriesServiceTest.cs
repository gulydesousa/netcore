using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Entities;
using FluentAssertions;
using AutoFixture;
using Moq;
using RepositoryContracts;
using Xunit.Abstractions;

namespace CRUDTests
{
    /// <summary>
    /// Test class for the CountriesService.
    /// </summary>
    public class CountriesServiceTest
    {
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        private readonly ICountriesService _countriesService;

        /// <summary>
        /// Constructor for the CountriesServiceTest class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public CountriesServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;

            // Create services based on mocked DbContext object
            _countriesService = new CountriesService(_countriesRepository);

            _testOutputHelper = testOutputHelper;
        }

        #region AddCountry

        /// <summary>
        /// Test method for the AddCountry method when the CountryAddRequest is null.
        /// It should throw an ArgumentNullException.
        /// </summary>
        [Fact]
        public async Task AddCountry_CountryIsNull_ToBeArgumentNullException()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = null;

            // Assert
            Func<Task> action = (async () =>
            {
                await _countriesService.AddCountry(countryAddRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// Test method for the AddCountry method when the CountryName is null.
        /// It should throw an ArgumentException.
        /// </summary>
        [Fact]
        public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
        {
            // Arrange
            CountryAddRequest country = _fixture.Build<CountryAddRequest>()
                .With(t => t.CountryName, null as string)
                .Create();

            // Assert
            Func<Task> action = (async () =>
            {
                await _countriesService.AddCountry(country);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// Test method for the AddCountry method when the CountryName is duplicated.
        /// It should throw an ArgumentException.
        /// </summary>
        [Fact]
        public async Task AddCountry_CountryNameIsDuplicated_ToBeArgumentException()
        {
            // Arrange
            Country country = _fixture.Build<Country>()
                .With(t => t.Persons, null as List<Person>)
                .Create();

            CountryAddRequest country_add_request = new CountryAddRequest { CountryName = country.CountryName };

            _countriesRepositoryMock.Setup(t => t.GetCountryByName(It.IsAny<string>()))
                .ReturnsAsync(country);

            // Assert
            Func<Task> action = (async () =>
            {
                await _countriesService.AddCountry(country_add_request);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// Test method for the AddCountry method when the CountryName is valid.
        /// It should successfully insert (add) the country to the list of countries.
        /// </summary>
        [Fact]
        public async Task AddCountry_CountryNameIsValid_ToBeSuccessful()
        {
            // Arrange
            Country country = _fixture.Build<Country>()
                .With(t => t.Persons, null as List<Person>)
                .Create();

            CountryAddRequest country_add_request = new CountryAddRequest { CountryName = country.CountryName };
            CountryResponse country_expected_response = country.ToCountryResponse();

            _countriesRepositoryMock.Setup(t => t.GetCountryByName(It.IsAny<string>()))
                .ReturnsAsync(null as Country);

            _countriesRepositoryMock.Setup(t => t.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);

            // Act
            CountryResponse country_response = await _countriesService.AddCountry(country_add_request);

            // Assert
            country.Id.Should().NotBeEmpty();
            country.Id.Should().NotBe(new Guid());
        }

        #endregion AddCountry

        #region GetAllCountries

        /// <summary>
        /// Test method for the GetCountries method when there are no countries.
        /// It should return an empty list.
        /// </summary>
        [Fact]
        public async Task GetCountries_NoCountries_ToBeEmpty()
        {
            // Arrange
            List<Country> countries = new List<Country>();

            _countriesRepositoryMock.Setup(t => t.GetCountries())
                .ReturnsAsync(countries);

            // Act
            IList<CountryResponse> result = await _countriesService.GetCountries();

            // Assert
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Test method for the GetCountries method when there are countries.
        /// It should return a list of countries.
        /// </summary>
        [Fact]
        public async Task GetCountries_ThereAreCountries_ToBeSuccessful()
        {
            // Arrange
            List<Country> countries = new List<Country>{
                _fixture.Build<Country>()
                    .With(t => t.Persons, null as List<Person>)
                    .Create(),

                _fixture.Build<Country>()
                    .With(t => t.Persons, null as List<Person>)
                    .Create(),

                _fixture.Build<Country>()
                    .With(t => t.Persons, null as List<Person>)
                    .Create(),
            };

            List<CountryResponse> expected_country_response = countries.Select(x => x.ToCountryResponse()).ToList();

            _countriesRepositoryMock.Setup(t => t.GetCountries())
                .ReturnsAsync(countries);

            // Act
            IList<CountryResponse> result = await _countriesService.GetCountries();

            // Assert
            // All countries added should be in the list of countries
            result.Should().BeEquivalentTo(expected_country_response);
        }

        #endregion GetCountriesº

        #region GetCountryById

        /// <summary>
        /// Test method for the GetCountryById method when the CountryId is null.
        /// It should throw an ArgumentNullException.
        /// </summary>
        [Fact]
        public async Task GetCountryById_CountryIdIsNull_ToBeArgumentNullException()
        {
            // Arrange
            Country country = _fixture.Build<Country>()
                .With(t => t.Persons, null as List<Person>)
                .Create();

            _countriesRepositoryMock.Setup(t => t.GetCountryById(It.IsAny<Guid?>()))
                .ReturnsAsync((Country?)null);

            // Assert
            Func<Task> action = (async () =>
            {
                await _countriesService.GetCountryById(null);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        /// <summary>
        /// Test method for the GetCountryById method when the CountryId is valid.
        /// It should return the country.
        /// </summary>
        [Fact]
        public async Task GetCountryById_CountryIdIsValid_ToBeSuccessful()
        {
            // Arrange
            Country country = _fixture.Build<Country>()
                .With(t => t.Persons, null as List<Person>)
                .Create();

            CountryResponse country_expected_response = country.ToCountryResponse();

            _countriesRepositoryMock.Setup(t => t.GetCountryById(It.IsAny<Guid?>()))
                .ReturnsAsync(country);

            // Act
            CountryResponse? response = await _countriesService.GetCountryById(country.Id);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(country_expected_response);
        }

        /// <summary>
        /// Test method for the GetCountryById method when the CountryId is invalid.
        /// It should return null.
        /// </summary>
        [Fact]
        public async Task GetCountryById_CountryIdIsInvalid_toBeNull()
        {
            // Arrange
            _countriesRepositoryMock.Setup(t => t.GetCountryById(It.IsAny<Guid?>()))
                .ReturnsAsync(null as Country);

            // Act
            Func<Task> action = (async () =>
            {
                await _countriesService.GetCountryById(null);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        #endregion GetCountryById
    }
}
