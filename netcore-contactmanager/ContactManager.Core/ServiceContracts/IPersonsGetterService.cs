using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsGetterService
    {
        /// <summary>   
        /// Returns all persons
        ///</summary>
        ///<returns>Returns a list objects of PersonsResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns a person object based on  the given PersonID
        /// </summary>
        /// <param name="personID">Person it to search</param>
        /// <returns>Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        ///  Returns all person objects that matches with given field and seach string   
        /// </summary>
        /// <param name="searchBy">Search field to seach</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>All matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Returns persons as Excel
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetPersonsEXCEL();
    }
}
