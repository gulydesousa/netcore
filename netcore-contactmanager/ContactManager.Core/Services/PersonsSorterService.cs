using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Reflection;
using Serilog;

namespace Services
{
    /// <summary>
    /// Service class for sorting persons.
    /// </summary>
    public class PersonsSorterService : IPersonsSorterService
    {
        //private field to store all persons.
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        //Constructor
        public PersonsSorterService(IPersonsRepository personsRepository
        , ILogger<PersonsSorterService> logger
        , IDiagnosticContext _diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            this._diagnosticContext = _diagnosticContext;
        }

        /// <summary>
        /// Gets the sorted list of persons based on the specified sorting criteria.
        /// </summary>
        /// <param name="allPersons">The list of all persons.</param>
        /// <param name="sortBy">The property to sort by.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>The sorted list of persons.</returns>
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            //Code scanning alerts #6: Log entries created from user input
            _logger.LogInformation("GetSortedPersons: {sortBy} {sortOrder}"
                , sortBy.Replace(Environment.NewLine, "")
                , sortOrder.ToString().Replace(Environment.NewLine, ""));

            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            return SortPersons(allPersons, sortBy, sortOrder);
        }

        private List<PersonResponse> SortPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            //Code scanning alerts #6: Log entries created from user input
            _logger.LogInformation("SortPersons:{sortBy}, {sortOrder}"
                , sortBy.Replace(Environment.NewLine, "")
                , sortOrder.ToString().Replace(Environment.NewLine, ""));

            //Check sortBy is a valid property of PersonResponse
            if (!typeof(PersonResponse).GetProperties().Any(x => x.Name == sortBy))
                throw new ArgumentException($"Invalid sortBy property: {sortBy}");

            PropertyInfo? property = typeof(PersonResponse).GetProperty(sortBy);

            if (property == null) return allPersons;

            if (property?.PropertyType.Name == "String")
            {
                if (sortOrder == SortOrderOptions.ASC)
                {
                    return allPersons.OrderBy(x => property.GetValue(x)?.ToString(), StringComparer.CurrentCultureIgnoreCase).ToList();
                }
                else
                {
                    return allPersons.OrderByDescending(x => property.GetValue(x)?.ToString(), StringComparer.CurrentCultureIgnoreCase).ToList();
                }
            }
            else
            {
                if (sortOrder == SortOrderOptions.ASC)
                {
                    return allPersons.OrderBy(x => property?.GetValue(x)).ToList();
                }
                else
                {
                    return allPersons.OrderByDescending(x => property?.GetValue(x)).ToList();
                }
            }

        }
    }
}
