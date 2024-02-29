using AutoFixture;
using Entities;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using CRUDexample.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;

        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ICountriesService> countriesServiceMock;

        private readonly Mock<IPersonsAdderService> personsAdderServiceMock;
        private readonly Mock<IPersonsDeleterService> personsDeleterServiceMock;
        private readonly Mock<IPersonsGetterService> personsGetterServiceMock;
        private readonly Mock<IPersonsSorterService> personsSorterServiceMock;
        private readonly Mock<IPersonsUpdaterService> personsUpdaterServiceMock;


        private readonly Fixture _fixture;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();

            countriesServiceMock = new Mock<ICountriesService>();

            personsAdderServiceMock = new Mock<IPersonsAdderService>();
            personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
            personsGetterServiceMock = new Mock<IPersonsGetterService>();
            personsSorterServiceMock = new Mock<IPersonsSorterService>();
            personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();


            _personsAdderService = personsAdderServiceMock.Object;
            _personsDeleterService = personsDeleterServiceMock.Object;
            _personsGetterService = personsGetterServiceMock.Object;
            _personsSorterService = personsSorterServiceMock.Object;
            _personsUpdaterService = personsUpdaterServiceMock.Object;

            _countriesService = countriesServiceMock.Object;

            _logger = new Mock<ILogger<PersonsController>>().Object;
        }

        #region  Index
        /// <summary>
        /// Test for index method
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfPersons()
        {
            // Arrange
            List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();
            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetFilteredPersons and GetSortedPersons methods services form Index method 
            personsGetterServiceMock.Setup(service => service.GetFilteredPersons(It.IsAny<string>()
            , It.IsAny<string>()))
            .ReturnsAsync(persons_response_list);

            personsSorterServiceMock.Setup(service => service.GetSortedPersons(It.IsAny<List<PersonResponse>>()
            , It.IsAny<string>()
            , It.IsAny<SortOrderOptions>()))
            .Returns(persons_response_list);

            // Act
            IActionResult result =
            await personsController.Index(_fixture.Create<string>()
                                        , _fixture.Create<string>()
                                        , _fixture.Create<string>()
                                        , _fixture.Create<SortOrderOptions>());

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(persons_response_list);

        }

        #region  Create
        /// <summary>
        /// Test for Create HttpPost
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Create_ReturnsARedirectAndAddsPerson_WhenModelStateIsValid()
        {
            // Arrange
            PersonAddRequest personAddRequest =
            _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse =
            _fixture.Create<PersonResponse>();

            List<CountryResponse> countries =
            _fixture.Create<List<CountryResponse>>();

            PersonsController personsController =
            new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetCountrySelectListItems method service form Create method
            countriesServiceMock.Setup(service => service.GetCountries())
            .ReturnsAsync(countries);

            //Mocking the AddPerson method service form Create method 
            personsAdderServiceMock.Setup(service => service.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Create(personAddRequest);

            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        /// <summary>
        /// Test for Create HttpPost when ModelState is not valid
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact(Skip = "Irrelevante tras meter los ActionFilters")]
        public async Task Create_ReturnsAViewResult_WhenModelStateIsNotValid()
        {
            // Arrange
            PersonAddRequest personAddRequest =
            _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse =
            _fixture.Create<PersonResponse>();

            List<CountryResponse> countries =
            _fixture.Create<List<CountryResponse>>();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetCountrySelectListItems method service form Create method
            countriesServiceMock.Setup(service => service.GetCountries())
            .ReturnsAsync(countries);

            //Mocking the AddPerson method service form Create method 
            personsAdderServiceMock.Setup(service => service.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(personResponse);

            // Act
            personsController.ModelState.AddModelError("PersonName", "PersonName is required");
            IActionResult result = await personsController.Create(personAddRequest);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(personAddRequest);
            viewResult.ViewData["Countries"].Should().BeAssignableTo<IEnumerable<CountryResponse>>();
            viewResult.ViewData["Countries"].Should().Be(countries);
        }

        /// <summary>
        /// Test Create Method HttpGet
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Create_ReturnsAViewResult_WithAListOfCountries()
        {
            // Arrange
            PersonAddRequest personAddRequest =
            new PersonAddRequest();

            PersonResponse personResponse =
            _fixture.Create<PersonResponse>();

            List<CountryResponse> countries =
            _fixture.Create<List<CountryResponse>>();


            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);


            //Mocking the GetCountrySelectListItems method service form Create method
            countriesServiceMock.Setup(service => service.GetCountries())
            .ReturnsAsync(countries);



            //Mocking the AddPerson method service form Create method 
            personsAdderServiceMock.Setup(service => service.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Create();

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personAddRequest);
            viewResult.ViewData["Countries"].Should().BeAssignableTo<IEnumerable<SelectListItem>>();
        }

        #endregion Create

        #region Edit

        /// <summary>
        /// Test for Edit HttpGet
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Edit_ReturnsAViewResult_WithAListOfCountries()
        {
            // Arrange
            PersonResponse personResponse =
            _fixture.Create<PersonResponse>();

            PersonUpdateRequest personUpdateRequest =
            personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries =
            _fixture.Create<List<CountryResponse>>();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Edit method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            //Mocking the GetCountrySelectListItems method service form Edit method
            countriesServiceMock.Setup(service => service.GetCountries())
            .ReturnsAsync(countries);

            // Act
            IActionResult result = await personsController.Edit(personResponse.PersonID);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personUpdateRequest);
            viewResult.ViewData["Countries"].Should().BeAssignableTo<IEnumerable<SelectListItem>>();
        }

        /// <summary>
        /// Test for Edit HttpGet when personResponse is null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task EditGet_ReturnsARedirectToActionIndex_WhenPersonResponseIsNull()
        {
            // Arrange
            PersonResponse? personResponse = null;

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);


            //Mocking the GetPersonByPersonID method service form Edit method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Edit(_fixture.Create<Guid>());

            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        /// <summary>
        /// Test for Edit HttpPost when ModelState is valid
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task Edit_ReturnsARedirectToActionIndex_WhenModelStateIsValid()
        {
            // Arrange
            PersonResponse personResponse =
            _fixture.Create<PersonResponse>();

            PersonUpdateRequest personUpdateRequest =
            _fixture.Create<PersonUpdateRequest>();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Edit method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            //Mocking the UpdatePerson method service form Edit method
            personsUpdaterServiceMock.Setup(service => service.UpdatePerson(It.IsAny<PersonUpdateRequest>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Edit(personUpdateRequest);

            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        /// <summary>
        /// Test for Edit HttpPost when ModelState is not valid
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact(Skip = "Irrelevante tras meter los ActionFilters")]
        public async Task Edit_ReturnsAViewResult_WhenModelStateIsNotValid()
        {
            // Arrange

            PersonUpdateRequest personUpdateRequest =
            _fixture.Create<PersonUpdateRequest>();

            Person person =
            personUpdateRequest.ToPerson();

            PersonResponse personResponse =
            person.ToPersonResponse();


            List<CountryResponse> countries =
            _fixture.Create<List<CountryResponse>>();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Edit method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            //Mocking the GetCountrySelectListItems method service form Edit method
            countriesServiceMock.Setup(service => service.GetCountries())
            .ReturnsAsync(countries);

            // Act
            personsController.ModelState.AddModelError("PersonName", "PersonName is required");
            IActionResult result = await personsController.Edit(personUpdateRequest);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personUpdateRequest);
            viewResult.ViewData["Countries"].Should().BeAssignableTo<IEnumerable<CountryResponse>>();
            viewResult.ViewData["Countries"].Should().Be(countries);
        }

        /// <summary>
        /// Test for Edit HttpPost when personResponse is null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task EditPost_ReturnsARedirectToActionIndex_WhenPersonResponseIsNull()
        {
            // Arrange
            PersonResponse? personResponse = null;

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Edit method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Edit(_fixture.Create<PersonUpdateRequest>());

            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }


        #endregion Edit

        //Test for Delete HttpGet
        /// <summary>
        /// Test for the Delete HttpGet method.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsAViewResult_WithAPersonResponse()
        {
            // Arrange
            PersonResponse personResponse =
            _fixture.Create<PersonResponse>();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Delete method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Delete(personResponse.PersonID);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonResponse>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponse);
        }

        //Test for Delete HttpGet when personResponse is null
        /// <summary>
        /// Test for the Delete HttpGet method when personResponse is null.
        /// </summary>
        [Fact]
        public async Task DeleteGet_ReturnsARedirectToActionIndex_WhenPersonResponseIsNull()
        {
            // Arrange
            PersonResponse? personResponse = null;

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Delete method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            // Act
            IActionResult result = await personsController.Delete(_fixture.Create<Guid>());

            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        //Test for Delete HttpPost
        /// <summary>
        /// Test for the Delete HttpPost method.
        /// </summary>
        [Fact]
        public async Task DeleteConfirmed_ReturnsARedirectToActionIndex_WhenPersonResponseIsNotNull()
        {
            // Arrange
            PersonUpdateRequest personUpdateRequest =
            _fixture.Create<PersonUpdateRequest>();

            Person person =
            personUpdateRequest.ToPerson();

            PersonResponse personResponse =
            person.ToPersonResponse();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Delete method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
           .ReturnsAsync(personResponse);

            //Mocking the DeletePerson method service form Delete method
            personsDeleterServiceMock.Setup(service => service.DeletePerson(It.IsAny<Guid>()))
            .ReturnsAsync(true);

            // Act
            IActionResult result = await personsController.Delete(personUpdateRequest);

            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        //Test for Delete HttpPost when personResponse is null
        /// <summary>
        /// Test for the Delete HttpPost method when personResponse is null.
        /// </summary>
        [Fact]
        public async Task DeleteConfirmedPost_ReturnsARedirectToActionIndex_WhenPersonResponseIsNull()
        {
            // Arrange
            PersonUpdateRequest personUpdateRequest =
            _fixture.Create<PersonUpdateRequest>();

            Person person =
            personUpdateRequest.ToPerson();

            PersonResponse? personResponse = null;

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Delete method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            //Mocking the DeletePerson method service form Delete method
            personsDeleterServiceMock.Setup(service => service.DeletePerson(It.IsAny<Guid>()))
            .ReturnsAsync(true);

            // Act
            IActionResult result = await personsController.Delete(personUpdateRequest);
            // Assert
            RedirectToActionResult redirectToActionResult =
            Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        //Test for Delete HttpPost when personResponse is not null and DeletePerson method returns false
        /// <summary>
        /// Test for the Delete HttpPost method when personResponse is not null and DeletePerson method returns false.
        /// </summary>
        [Fact]
        public async Task DeleteConfirmedPost_ReturnsARedirectToActionIndex_WhenDeletePersonReturnsFalse()
        {
            // Arrange
            PersonUpdateRequest personUpdateRequest =
            _fixture.Create<PersonUpdateRequest>();

            Person person =
            personUpdateRequest.ToPerson();

            PersonResponse personResponse =
            person.ToPersonResponse();

            PersonsController personsController = new PersonsController(_personsAdderService
                , _personsDeleterService
                , _personsGetterService
                , _personsSorterService
                , _personsUpdaterService
                , _countriesService
                , _logger);

            //Mocking the GetPersonByPersonID method service form Delete method
            personsGetterServiceMock.Setup(service => service.GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(personResponse);

            //Mocking the DeletePerson method service form Delete method
            personsDeleterServiceMock.Setup(service => service.DeletePerson(It.IsAny<Guid>()))
            .ReturnsAsync(false);


            // Act
            IActionResult result = await personsController.Delete(personResponse.PersonID);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonResponse>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponse);
        }



        #endregion Delete



    }


}
