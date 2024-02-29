using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsSorterService
    {   
        /// <summary>
        /// Returns sorted list of persons    
        /// </summary>
        /// <param name="allPersons">Represents a list of persons to sort</param>
        /// <param name="sortBy">Name of the property(key), based on which persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonsResponseList</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

    }
}
