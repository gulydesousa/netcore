using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Serilog;
using Exceptions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;
        
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        
        private readonly ILogger<PersonsAdderService> _loggerAdder;
        private readonly ILogger<PersonsDeleterService> _loggerDeleter;
        private readonly ILogger<PersonsGetterService> _loggerGetter;
        private readonly ILogger<PersonsSorterService> _loggerSorter;
        private readonly ILogger<PersonsUpdaterService> _loggerUpdater;

        private readonly IDiagnosticContext _diagnosticContext;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;

            _loggerAdder = new Mock<ILogger<PersonsAdderService>>().Object;
            _loggerDeleter = new Mock<ILogger<PersonsDeleterService>>().Object;
            _loggerGetter = new Mock<ILogger<PersonsGetterService>>().Object;
            _loggerSorter = new Mock<ILogger<PersonsSorterService>>().Object;
            _loggerUpdater = new Mock<ILogger<PersonsUpdaterService>>().Object;
            
            _diagnosticContext = new Mock<IDiagnosticContext>().Object;

            //Create services based on mocked DbContext object
            _personsAdderService = new PersonsAdderService(_personsRepository,_loggerAdder, _diagnosticContext);
            _personsDeleterService = new PersonsDeleterService(_personsRepository,_loggerDeleter, _diagnosticContext);
            _personsGetterService = new PersonsGetterService(_personsRepository,_loggerGetter, _diagnosticContext);
            _personsSorterService = new PersonsSorterService(_personsRepository,_loggerSorter, _diagnosticContext);
            _personsUpdaterService = new PersonsUpdaterService(_personsRepository,_loggerUpdater, _diagnosticContext);
            
            _testOutputHelper = testOutputHelper;
        }

      
        private void PrintPersonResponseList(List<PersonResponse> resultList, ITestOutputHelper testOutputHelper, string text)
        {
            testOutputHelper.WriteLine($"**** {text} ****");
            foreach (PersonResponse item in resultList)
            {
                int index = resultList.IndexOf(item);
                _testOutputHelper.WriteLine($"{index}. {item.ToString()}");
            }
        }


        #region AddPerson

        /// <summary>
        /// When we supply a null value as PersonAddRequest, it should throw an ArgumentNullException
        /// </summary>
        [Fact]
        public async Task AddPerson_NullPersonRequest_ToBeArgumentNullException()
        {
            //Arrange
            PersonAddRequest? personRequest = null;

            //Assert
            Func<Task> action = (async () =>
            {
                await _personsAdderService.AddPerson(personRequest);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        /// <summary>
        /// When we supply a PersonAddRequest with null PersonName, it should throw an ArgumentException
        /// </summary>
        [Fact]
        public async Task AddPerson_NullPersonName_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                                           .With(temp => temp.PersonName, null as string)
                                           .Create();

            ///If we supply any argument value to the AddPerson methid, it should return the same value
            Person person = personRequest.ToPerson();
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            //Assert
            Func<Task> action = (async () =>
            {
                await _personsAdderService.AddPerson(personRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When we supply a PersonAddRequest with null Email, it should throw an ArgumentException
        /// </summary>
        [Fact]
        public async Task AddPerson_NullEmail_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                                           .With(temp => temp.Email, null as string)
                                           .Create();

            ///If we supply any argument value to the AddPerson methid, it should return the same value
            Person person = personRequest.ToPerson();
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            //Assert
            Func<Task> action = (async () =>
            {
                await _personsAdderService.AddPerson(personRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }


        /// <summary>
        /// When we supply a PersonAddRequest with empty Email, it should throw an ArgumentException
        /// </summary>
        [Fact]
        public async Task AddPerson_EmptyEmail_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                                            .With(temp => temp.Email, string.Empty)
                                            .Create();

            ///If we supply any argument value to the AddPerson methid, it should return the same value
            Person person = personRequest.ToPerson();
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);


            //Assert
            Func<Task> action = (async () =>
            {
                await _personsAdderService.AddPerson(personRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }


        /// <summary>
        /// When we supply proper person details, it should return the same person details,
        /// along with the newly created PersonID
        /// </summary>
        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccesful()
        {
            //Arrange
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                                            .With(temp => temp.Email, "person@email.com")
                                            .With(temp => temp.DateOfBirth, new DateTime(1980, 1, 1))
                                            .Create();



            ///If we supply any argument value to the AddPerson methid, it should return the same value
            Person person = personRequest.ToPerson();
            PersonResponse personResponse_expected = person.ToPersonResponse();
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);


            //Act
            PersonResponse personResponse = await _personsAdderService.AddPerson(personRequest);

            personResponse_expected.PersonID = personResponse.PersonID;

            //Assert
            personResponse.PersonID.Should().NotBe(Guid.Empty);
            personResponse.Should().Be(personResponse_expected);
        }


        [Fact]
        public async Task AddPerson_Fails_ReturnsException()
        {
            // Arrange
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                                             .With(temp => temp.Email, "person@email.com")
                                             .With(temp => temp.DateOfBirth, new DateTime(1980, 1, 1))
                                             .Create();

            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _personsAdderService.AddPerson(personRequest));
        }


        [Fact]
        public async Task AddPerson_AlreadyExists_ReturnsConflictException()
        {
            // Arrange
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                                           .With(temp => temp.Email, "person@email.com")
                                           .With(temp => temp.DateOfBirth, new DateTime(1980, 1, 1))
                                           .Create();

            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ThrowsAsync(new ConflictException("La persona ya existe en la base de datos."));

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _personsAdderService.AddPerson(personRequest));
        }


        #endregion AddPerson

        #region GetAllPersons
        /// <summary>
        /// By default, the list of persons should be empty
        /// </summary>
        [Fact]
        public async Task GetAllPersons_EmptyList_ToBeEmptyList()
        {
            //Arrange

            //Mock Repository, return empty list
            List<Person> persons = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);

            //Act
            List<PersonResponse> personsList = await _personsGetterService.GetAllPersons();


            //Assert
            personsList.Should().BeEmpty();
        }

        /// <summary>
        /// When we add a person, it should be returned by GetAllPersons
        /// </summary>
        [Fact]
        public async Task GetAllPersons_AFewPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t => t.PersonName, "Jon Doe")
                .With(t => t.Email, "jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t => t.PersonName, "Jane Doehrty")
                .With(t => t.Email, "jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t => t.PersonName, "Ron American")
                .With(t => t.Email, "ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };


            List<PersonResponse> person_response_list_expected =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(person_response_list_expected, _testOutputHelper, "Input");

            //Mock Repository, return list of persons
            List<Person> personsList = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);


            //Act
            List<PersonResponse> personsGetList = await _personsGetterService.GetAllPersons();

            //*****************************
            //Print the list of Get All Persons
            PrintPersonResponseList(personsGetList, _testOutputHelper, "Output");

            //Assert
            //person_response_list_expected should be the same as personsGetList
            personsGetList.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion GetAllPersons

        #region GetPersonByPersonID
        //If we supply a null value as PersonID, it should  return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? personResponse =
            await _personsGetterService.GetPersonByPersonID(personID);

            //Assert
            personResponse.Should().BeNull();
        }

        //If we supply a PersonID that exists, it should return the person details
        [Fact]
        public async Task GetPersonByPersonID_ExistingPersonID_ToBeSuccessful()
        {
            //Trabajamos con la premisa de una bbdd vacía, para completar el objeto personResponse
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(t => t.Email, "person@email.com")
                .With(t => t.Country, null as Country)
                .Create();

            PersonResponse personResponse_expected = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            //Act
            PersonResponse? personResponse_from_get =
            await _personsGetterService.GetPersonByPersonID(person.PersonID);

            //Assert
            personResponse_from_get.Should().Be(personResponse_expected);
        }


        //If we supply a PersonID that does not exist, it should return null
        #endregion GetPersonByPersonID

        #region GetFilteredPersons

        //If the seach filter is empty and seach by PersonName, it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySeachText_ReturnAllPersons()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };


            List<PersonResponse> person_response_list_expected =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(person_response_list_expected, _testOutputHelper, "Input");

            //Mock Repository, return list of persons
            _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);


            List<Person> personsList = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);

            //Act
            List<PersonResponse> personsGetList =
            await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

            //*****************************
            //Print the list of Get All Persons
            PrintPersonResponseList(personsGetList, _testOutputHelper, "Output");

            //Assert
            //personsGetList should be the same as person_response_list_expected
            person_response_list_expected.Should().BeEquivalentTo(personsGetList);
        }

        //If the search supply non existing search field, it should return all persons
        [Fact]
        public async Task GetFilteredPersons_NonExistingSearchField_ToBeAllPersons()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };


            List<PersonResponse> person_response_list_expected =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(person_response_list_expected, _testOutputHelper, "Input");

            //Mock Repository, return list of persons
            _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            List<Person> personsList = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);


            //Act
            List<PersonResponse> personsGetList =
                await _personsGetterService.GetFilteredPersons("NonExistingSearchField", "John");
            //*****************************
            //Print the list of Get All Persons
            PrintPersonResponseList(personsGetList, _testOutputHelper, "Output");

            //Assert
            //personsGetList should be the same as person_response_list_expected
            person_response_list_expected.Should().BeEquivalentTo(personsGetList);
        }

        //First we will add a few persons, then we will search for a person by PersonName
        [Fact]
        public async Task GetFilteredPersons_SeachByPersonName_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<Person> filteredPersons = persons.Where(p => p.PersonName.Contains("Doe")).ToList();

            List<PersonResponse> person_response_list_expected =
                filteredPersons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(person_response_list_expected, _testOutputHelper, "Input");

            //Mock Repository, return list of persons
            _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(filteredPersons);


            List<Person> personsList = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);


            //Act
            List<PersonResponse> personsGetList =
            await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "Doe");

            //*****************************
            //Print the list of GetFilteredPersons
            PrintPersonResponseList(personsGetList, _testOutputHelper, "Output");

            //Assert
            //personsGetList should be the same as person_response_list_expected
            person_response_list_expected.Should().BeEquivalentTo(personsGetList);
        }

        //First we will add a few persons, then we will search for a person date of birth with format dd-MM-yyyy returns a list of persons
        [Fact]
        public async Task GetFilteredPersons_SeachByDateOfBirth_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t=> t.DateOfBirth, new DateTime(1980, 1, 1))
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t=> t.DateOfBirth, new DateTime(1995, 11, 2))
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t=> t.DateOfBirth, new DateTime(2000, 5, 10))
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<Person> filteredPersons = persons.Where(p => p.DateOfBirth == new DateTime(1980, 1, 1)).ToList();

            List<PersonResponse> person_response_list_expected =
                filteredPersons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(person_response_list_expected, _testOutputHelper, "Input");

            //Mock Repository, return list of persons
            _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(filteredPersons);


            List<Person> personsList = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);


            //Act
            List<PersonResponse> personsGetList =
            await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "01/01/1980");

            //*****************************
            //Print the list of GetFilteredPersons
            PrintPersonResponseList(personsGetList, _testOutputHelper, "Output");

            //Assert
            //personsGetList should be the same as person_response_list_expected
            person_response_list_expected.Should().BeEquivalentTo(personsGetList);
        }


        //First we will add a few persons, then we will search for a person date of birth with format not equals dd-MM-yyyy returns a empty list of persons
        [Fact]
        public async Task GetFilteredPersons_SeachByDateOfBirthInvalidFormat_ToBeEmptyList()
        {
            //Arrange
            List<Person> persons = new List<Person>();

            List<PersonResponse> person_response_list_expected =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(person_response_list_expected, _testOutputHelper, "Input");

            //Mock Repository, return list of persons
            _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);


            List<Person> personsList = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                                  .ReturnsAsync(persons);


            //Act
            List<PersonResponse> personsGetList =
            await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "25/25/2025");

            //*****************************
            //Print the list of GetFilteredPersons
            PrintPersonResponseList(personsGetList, _testOutputHelper, "Output");

            //Assert
            //personsGetList should be the same as person_response_list_expected
            person_response_list_expected.Should().BeEmpty();
        }


        #endregion GetFilteredPersons

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return the list of persons sorted by PersonName on DESC order
        [Fact]
        public void GetSortedPersons_ByPersonNameDESC_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Act
            List<PersonResponse> personslistDesc =
            _personsSorterService.GetSortedPersons(allPersons, nameof(Person.PersonName), ServiceContracts.Enums.SortOrderOptions.DESC);

            PrintPersonResponseList(personslistDesc, _testOutputHelper, "Output");

            //Assert
            //Check if the list is sorted by PersonName on DESC order
            personslistDesc.Should().BeInDescendingOrder(t => t.PersonName);
        }

        //When we sort based on PersonName in ASC, it should return the list of persons sorted by PersonName on ACS order
        [Fact]
        public void GetSortedPersons_ByPersonNameASC_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Act
            List<PersonResponse> personslistDesc =
            _personsSorterService.GetSortedPersons(allPersons, nameof(Person.PersonName), ServiceContracts.Enums.SortOrderOptions.ASC);

            PrintPersonResponseList(personslistDesc, _testOutputHelper, "Output");

            //Assert
            //Check if the list is sorted by PersonName on ASC order
            personslistDesc.Should().BeInAscendingOrder(t => t.PersonName);
        }

        //When we sort based on DateOfBirth in DESC, it should return the list of persons sorted by DateOfBirth on DESC order
        [Fact]
        public void GetSortedPersons_ByDateOfBirthDESC_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Act
            List<PersonResponse> personslistDesc =
            _personsSorterService.GetSortedPersons(allPersons, nameof(Person.DateOfBirth), ServiceContracts.Enums.SortOrderOptions.DESC);

            PrintPersonResponseList(personslistDesc, _testOutputHelper, "Output");

            //Assert
            //Check if the list is sorted by PersonName on DESC order
            personslistDesc.Should().BeInDescendingOrder(t => t.DateOfBirth);
        }

        //When we sort based on DateOfBirth in ASC, it should return the list of persons sorted by DateOfBirth on ASC order
        [Fact]
        public async Task GetSortedPersons_ByDateOfBirthASC_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Act
            List<PersonResponse> personslistDesc =
            _personsSorterService.GetSortedPersons(allPersons, nameof(Person.DateOfBirth), ServiceContracts.Enums.SortOrderOptions.ASC);

            PrintPersonResponseList(personslistDesc, _testOutputHelper, "Output");

            //Assert
            //Check if the list is sorted by PersonName on ASC order
            personslistDesc.Should().BeInAscendingOrder(t => t.DateOfBirth);
        }

        //When we sort based on Gender in DESC, it should return the list of persons sorted by Gender on DESC order
        [Fact]
        public void GetSortedPersons_ByGenderDESC_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Act
            List<PersonResponse> personslistDesc =
            _personsSorterService.GetSortedPersons(allPersons, nameof(Person.Gender), ServiceContracts.Enums.SortOrderOptions.DESC);

            PrintPersonResponseList(personslistDesc, _testOutputHelper, "Output");

            //Assert
            //Check if the list is sorted by PersonName on DESC order
            personslistDesc.Should().BeInDescendingOrder(t => t.Gender);
        }

        //When we sort based on Gender in ASC, it should return the list of persons sorted by Gender on ASC order
        [Fact]
        public void GetSortedPersons_ByGenderAsc_ToBeSuccesful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Act
            List<PersonResponse> personslistDesc =
            _personsSorterService.GetSortedPersons(allPersons, nameof(Person.Gender), ServiceContracts.Enums.SortOrderOptions.ASC);

            PrintPersonResponseList(personslistDesc, _testOutputHelper, "Output");

            //Assert
            //Check if the list is sorted by PersonName on ASC order
            personslistDesc.Should().BeInAscendingOrder(t => t.Gender);
        }

        //When we sort based on unexisting property it returns ArgumentException
        [Fact]
        public void GetSortedPersons_ByUnexistingProperty_ToBeArgumentException()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jon Doe")
                .With(t=>t.Email ,"jon@gmail.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Jane Doehrty")
                .With(t=>t.Email ,"jane@yahoo.com")
                .With(t => t.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(t=>t.PersonName ,"Ron American")
                .With(t=>t.Email ,"ron@hotmail.com")
                .With(t => t.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> allPersons =
                persons.Select(t => t.ToPersonResponse()).ToList();

            PrintPersonResponseList(allPersons, _testOutputHelper, "Input");

            //Assert
            Action action = () => _personsSorterService.GetSortedPersons(allPersons, "UnexistingProperty", ServiceContracts.Enums.SortOrderOptions.ASC);

            action.Should().Throw<ArgumentException>();
        }


        #endregion GetSortedPersons

        #region UpdatePerson
        //When we supply a null value as PersonUpdateRequest, it should throw an ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            //Arrange
            PersonUpdateRequest? personRequest = null;

            //Assert
            Func<Task> action = (async () =>
            {
                await _personsUpdaterService.UpdatePerson(personRequest);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply a PersonUpdateRequest with null PersonName, it should throw an ArgumentException
        [Fact]
        public async Task UpdatePerson_NullPersonName_ToBeArgumentException()
        {
            //Arrange
            Person person
                = _fixture.Build<Person>()
                .With(t => t.PersonName, null as string)
                .With(t => t.Email, "persons@gmail.com")
                .With(t => t.Country, null as Country)
                .Create();

            PersonResponse person_response = person.ToPersonResponse();
            PersonUpdateRequest person_update_request =
                person_response.ToPersonUpdateRequest();

            //Act
            Func<Task> action = (async () =>
            {
                await _personsUpdaterService.UpdatePerson(person_update_request);
            });
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply a PersonUpdateRequest with null PersonID, it should throw an ArgumentException
        [Fact]
        public async Task UpdatePerson_NullPersonID_ToBeArgumentException()
        {
            //Arrange
            Person person
                = _fixture.Build<Person>()
                .With(t => t.PersonName, "Jon Doe")
                .With(t => t.Email, "persons@gmail.com")
                .With(t => t.Country, null as Country)
                .Create();

            PersonResponse person_response = person.ToPersonResponse();
            PersonUpdateRequest person_update_request =
                person_response.ToPersonUpdateRequest();


            person_update_request.PersonID = null;

            //Act
            Func<Task> action = (async () =>
            {
                await _personsUpdaterService.UpdatePerson(person_update_request);
            });
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply a PersonUpdateRequest with a PersonID that does not exist, it should throw an ArgumentException
        [Fact]
        public async Task UpdatePerson_NonExistingPersonID_ToBeArgumentException()
        {
            //Arrange
            Person person
               = _fixture.Build<Person>()
               .With(t => t.PersonName, "Jon Doe")
               .With(t => t.Email, "persons@gmail.com")
               .With(t => t.Country, null as Country)
               .Create();

            PersonResponse person_response = person.ToPersonResponse();
            PersonUpdateRequest person_update_request =
                person_response.ToPersonUpdateRequest();


            //Act
            Func<Task> action = (async () =>
            {
                await _personsUpdaterService.UpdatePerson(person_update_request);
            });
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply a PersonUpdateRequest with a PersonID that exists, it should update the person details
        [Fact]
        public async Task UpdatePerson_ExistingPersonID_ToBeSuccesful()
        {

            //Arrange       
            Person person
               = _fixture.Build<Person>()
               .With(t => t.PersonName, "Jon Doe")
               .With(t => t.Email, "persons@gmail.com")
               .With(t => t.Country, null as Country)
               .Create();

            PersonResponse person_response = person.ToPersonResponse();
            PersonUpdateRequest person_update_request =
                person_response.ToPersonUpdateRequest();

            person_update_request.PersonName = "John Doe Updated";
            person_update_request.Email = "jondoe@gemeil.com";
            person_update_request.Address = "123 Main St Updated";
            person_update_request.DateOfBirth = new DateTime(1980, 12, 31);
            person_update_request.ReceiveNewsLetter = false;

            Person person_updated = person_update_request.ToPerson();
            PersonResponse person_response_expected = person_updated.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
           .ReturnsAsync(person);

            _personsRepositoryMock.Setup(t => t.UpdatePerson(It.IsAny<Person>()))
             .ReturnsAsync(person_updated);

            _testOutputHelper.WriteLine("*** Before *****");
            _testOutputHelper.WriteLine(person_update_request.ToString());

            //Act
            PersonResponse personResponseUpdate = await _personsUpdaterService.UpdatePerson(person_update_request);

            _testOutputHelper.WriteLine("*** After *****");
            _testOutputHelper.WriteLine(personResponseUpdate.ToString());

            //Assert
            personResponseUpdate.Should().BeEquivalentTo(person_updated.ToPersonResponse());
        }

        #endregion UpdatePerson

        #region DeletePerson
        //When we supply a null value as PersonID, it should throw an ArgumentNullException
        [Fact]
        public async Task DeletePerson_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Assert
            Func<Task> action = (async () =>
            {
                await _personsDeleterService.DeletePerson(personID);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply a PersonID that does not exist, it should return false
        [Fact]
        public async Task DeletePerson_NonExistingPersonID_ToBeFalse()
        {
            //Arrange
            Person person
              = _fixture.Build<Person>()
              .With(t => t.PersonName, "Jon Doe")
              .With(t => t.Email, "persons@gmail.com")
              .With(t => t.Country, null as Country)
              .Create();


            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
           .ReturnsAsync(null as Person);

            _testOutputHelper.WriteLine("*** Before *****");
            _testOutputHelper.WriteLine(person.ToString());


            //Act
            bool isDeleted = await _personsDeleterService.DeletePerson(person.PersonID);

            //Assert
            isDeleted.Should().BeFalse();
        }

        //When we supply a PersonID that exists, it should return true
        [Fact]
        public async Task DeletePerson_ExistingPersonID_ToBeTrue()
        {

            //Arrange
            Person person
              = _fixture.Build<Person>()
              .With(t => t.PersonName, "Jon Doe")
              .With(t => t.Email, "persons@gmail.com")
              .With(t => t.Country, null as Country)
              .Create();


            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
           .ReturnsAsync(person);

            _personsRepositoryMock.Setup(temp => temp.DeletePerson(It.IsAny<Guid>()))
            .ReturnsAsync(true);

            _testOutputHelper.WriteLine("*** Before *****");
            _testOutputHelper.WriteLine(person.ToString());

            //Act
            bool isDeleted = await _personsDeleterService.DeletePerson(person.PersonID);

            //Assert
            isDeleted.Should().BeTrue();
        }
        #endregion DeletePerson


    }
}
