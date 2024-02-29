using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsUpdaterService
    {  
        /// <summary>
        /// Updates the person details based on the given PersonID
        /// </summary>
        ///<param name="personRequest">Person details to update, including person id</param>
        ///<returns>Returns the updated person details</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personRequest);

    }
}
