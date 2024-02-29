using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using Serilog;

namespace Services
{
    public class PersonsDeleterService : IPersonsDeleterService
    {
        //private field to store all persons.
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        //Constructor
        public PersonsDeleterService(IPersonsRepository personsRepository
        , ILogger<PersonsDeleterService> logger
        , IDiagnosticContext _diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            this._diagnosticContext = _diagnosticContext;
        }

        /// <summary>
        /// Deletes a person based on the provided person ID.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns>True if the person was deleted successfully, otherwise false.</returns>
        public async Task<bool> DeletePerson(Guid? personID)
        {
            //Check if "personID" is not null. 
            if (personID == null)
            {
                throw new ArgumentNullException();
            }

            //Get the matching "Person" object from List<Person> based on PersonID. 
            Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);


            //Check if matching "Person" object is not null 
            if (person == null) return false;

            //Delete the matching "Person" object from List<Person> 
            await _personsRepository.DeletePerson(personID);

            //Return Boolean value indicating whether person object was deleted or not 
            return true;
        }

    }
}
