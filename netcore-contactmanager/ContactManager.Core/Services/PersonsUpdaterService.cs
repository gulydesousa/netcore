using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using Serilog;
using Exceptions;

namespace Services
{
    /// <summary>
    /// Service class for updating persons.
    /// </summary>
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        //private field to store all persons.
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        //Constructor
        public PersonsUpdaterService(IPersonsRepository personsRepository
        , ILogger<PersonsUpdaterService> logger
        , IDiagnosticContext _diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            this._diagnosticContext = _diagnosticContext;
        }

        /// <summary>
        /// Updates a person based on the provided person update request.
        /// </summary>
        /// <param name="personRequest">The person update request.</param>
        /// <returns>The updated person response.</returns>
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personRequest)
        {
            //Check if "personUpdateRequest is not null.
            if (personRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validate all properties of "personUpdateRequest" 
            ValidationHelper.ModelValidation(personRequest);

            //Get the matching "Person" object from List<Person> based on PersonlD. 
            Person? person = await _personsRepository.GetPersonByPersonID(personRequest.PersonID);

            //Check if matching "Person object is not null 
            if (person == null)
            {
                throw new InvalidPersonIDException($"Invalid PersonID: {personRequest.PersonID}");
            }

            //Update all details from "PersonUpdateRequest" object to "Person" object            
            person.PersonName = personRequest.PersonName;
            person.Email = personRequest.Email;
            person.DateOfBirth = personRequest.DateOfBirth;
            person.Address = personRequest.Address;
            person.CountryID = personRequest.CountryID;
            person.Gender = personRequest.Gender.ToString();
            person.ReceiveNewsLetter = personRequest.ReceiveNewsLetter;

            await _personsRepository.UpdatePerson(person);

            //Return PersonResponse object with updated details 
            return person.ToPersonResponse();
        }
    }
}
