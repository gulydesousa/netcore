using System.Linq.Expressions;
using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managind Person Entity 
    /// </summary>
    public interface IPersonsRepository
    {

        /// <summary>
        /// Adds person object to the datastore
        /// </summary>
        /// <param name="person">Person object to add</param>
        /// <returns>Returns the person object after adding it to the table</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Returns all persons in the data strore
        /// </summary>
        /// <returns>List of persons objects from table</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Retiurns a person by its Id
        /// </summary>
        /// <param name="personId">Person ID guid to searh</param>
        /// <returns>A person object or null</returns>
        Task<Person?> GetPersonByPersonID(Guid? personId);

        /// <summary>
        /// Returns all persons matching the filter
        /// </summary>
        /// <param name="filter">Linq expression to check</param>
        /// <returns>All matching with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> filter);

        /// <summary>
        ///  Deletes a person from the data store
        /// </summary>
        /// <param name="personId">Person ID (guid) to delete</param>
        /// <returns>true if deleted otherwise false</returns>
        Task<bool> DeletePerson(Guid? personId);

        /// <summary>
        /// Updates a person in the data store
        /// </summary>
        /// <param name="person">Person object ot update</param>
        /// <returns>Returns updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
