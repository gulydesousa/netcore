using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace CRUDTests
{
    /// <summary>
    /// Integration tests for the PersonsController class.
    /// </summary>
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsControllerIntegrationTest"/> class.
        /// </summary>
        /// <param name="factory">The custom web application factory.</param>
        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region Index
        /// <summary>
        /// Tests that the Index action returns a view result.
        /// </summary>
        [Fact(Skip = "Disabled for now, will fix later")]
        public async Task Index_ToReturnViewResult()
        {
            //Arrange


            //Act
            HttpResponseMessage result = await _client.GetAsync("/Persons/Index");

            string responseBody = await result.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            var document = html.DocumentNode;
            //table con el css persons
            document.QuerySelectorAll("table.persons").First().Should().NotBeNull();


            //Assert
            //result.Should().BeSuccessful();

        }
        #endregion
    }
}
