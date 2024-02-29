using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using Serilog;

namespace Services
{
    /// <summary>
    /// Service class for adding persons.
    /// </summary>
    public class PersonsAdderService : IPersonsAdderService
    {
        //private field to store all persons.
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        /// <summary>
        /// Constructor for PersonsAdderService.
        /// </summary>
        /// <param name="personsRepository">The repository for persons.</param>
        /// <param name="logger">The logger for logging.</param>
        /// <param name="_diagnosticContext">The diagnostic context.</param>
        public PersonsAdderService(IPersonsRepository personsRepository
        , ILogger<PersonsAdderService> logger
        , IDiagnosticContext _diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            this._diagnosticContext = _diagnosticContext;
        }

        /// <summary>
        /// Adds a person.
        /// </summary>
        /// <param name="personAddRequest">The person to add.</param>
        /// <returns>The response containing the added person.</returns>
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //Check if "personAddRequest" is not null. 
            if (personAddRequest == null)
            {
                throw new ArgumentNullException();
            }

            //Model Validations
            ValidationHelper.ModelValidation(personAddRequest);

            //Convert "personAddRequest" from "PersonAddRequest" type to "Person".
            Person person = personAddRequest.ToPerson();

            //Generate a new PersonlD. 
            person.PersonID = Guid.NewGuid();

            //await _countriesRepository.sp_PersonsInsert(person);
            //add person object to persons list
            await _personsRepository.AddPerson(person);

            //Return PersonResponse object with generated PersonlD. 
            PersonResponse personResponse = person.ToPersonResponse();

            return personResponse;
        }

    }
}
