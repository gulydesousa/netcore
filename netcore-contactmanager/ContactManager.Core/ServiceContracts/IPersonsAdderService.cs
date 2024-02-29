using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a new person into the list of Persons        
        /// </summary>
        /// <param name="personRequest">Person to add</param>
        /// <returns>Returns the same person details, along with the newly created PersonID</returns>
       Task<PersonResponse> AddPerson(PersonAddRequest? personRequest);

    }
}
