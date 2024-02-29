using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repository
{
    /// <summary>
    /// Repository class for managing persons.
    /// </summary>
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<PersonsRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="logger">The logger.</param>
        public PersonsRepository(ApplicationDbContext dbContext, ILogger<PersonsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new person to the database.
        /// </summary>
        /// <param name="person">The person to add.</param>
        /// <returns>The added person.</returns>
        /// <exception cref="ConflictException">Thrown if the person already exists in the database.</exception>
        public async Task<Person> AddPerson(Person person)
        {
            var existingPerson = await _dbContext.Persons.FirstOrDefaultAsync(p => p.PersonID == person.PersonID);

            if (existingPerson != null)
            {
                throw new ConflictException("La persona ya existe en la base de datos.");
            }

            _dbContext.Add(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }

        /// <summary>
        /// Deletes a person from the database.
        /// </summary>
        /// <param name="personId">The ID of the person to delete.</param>
        /// <returns>True if the person was deleted successfully, otherwise false.</returns>
        public async Task<bool> DeletePerson(Guid? personId)
        {
            _dbContext.Persons.RemoveRange(_dbContext.Persons.Where(p => p.PersonID == personId));
            int rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }

        /// <summary>
        /// Gets all persons from the database.
        /// </summary>
        /// <returns>A list of all persons.</returns>
        public async Task<List<Person>> GetAllPersons()
        {
            return await _dbContext.Persons.Include("Country").ToListAsync();
        }

        /// <summary>
        /// Gets filtered persons from the database based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <returns>A list of filtered persons.</returns>
        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> filter)
        {
            _logger.LogInformation("PersonsRepository.GetFilteredPersons() called");

            return await _dbContext.Persons.Include("Country").Where(filter).ToListAsync();
        }

        /// <summary>
        /// Gets a person from the database by their ID.
        /// </summary>
        /// <param name="personId">The ID of the person to retrieve.</param>
        /// <returns>The person with the specified ID, or null if not found.</returns>
        public async Task<Person?> GetPersonByPersonID(Guid? personId)
        {
            return await _dbContext.Persons.Include("Country").FirstOrDefaultAsync(p => p.PersonID == personId);
        }

        /// <summary>
        /// Updates a person in the database.
        /// </summary>
        /// <param name="person">The updated person.</param>
        /// <returns>The updated person.</returns>
        public async Task<Person> UpdatePerson(Person person)
        {
            Person? personToUpdate = await _dbContext.Persons.FirstOrDefaultAsync(p => p.PersonID == person.PersonID);

            if (personToUpdate == null)
            {
                return person;
            }

            personToUpdate.TIN = person.TIN;
            personToUpdate.PersonName = person.PersonName;
            personToUpdate.CountryID = person.CountryID;
            personToUpdate.Address = person.Address;
            personToUpdate.Email = person.Email;
            personToUpdate.ReceiveNewsLetter = person.ReceiveNewsLetter;
            personToUpdate.Gender = person.Gender;
            personToUpdate.DateOfBirth = person.DateOfBirth;

            await _dbContext.SaveChangesAsync();
            return personToUpdate;
        }
    }
}
