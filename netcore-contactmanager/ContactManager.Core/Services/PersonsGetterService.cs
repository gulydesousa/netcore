using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Globalization;
using Serilog;
using SerilogTimings;

namespace Services
{
    /// <summary>
    /// Service class for retrieving persons.
    /// </summary>
    public class PersonsGetterService : IPersonsGetterService
    {
        //private field to store all persons.
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        //Constructor
        public PersonsGetterService(IPersonsRepository personsRepository
        , ILogger<PersonsGetterService> logger
        , IDiagnosticContext _diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            this._diagnosticContext = _diagnosticContext;
        }

        /// <summary>
        /// Retrieves all persons.
        /// </summary>
        /// <returns>A list of PersonResponse objects.</returns>
        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons called");

            //Convert List<Person> to List<PersonResponse>

            List<Person> persons = await _personsRepository.GetAllPersons();

            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        /// <summary>
        /// Retrieves a person by their ID.
        /// </summary>
        /// <param name="personID">The ID of the person to retrieve.</param>
        /// <returns>A PersonResponse object if found, otherwise null.</returns>
        public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            //Check if "personlD" is not null
            if (personID == null) return null;

            //Get matching person from List<Person> based personlD. 
            Person? person = await _personsRepository.GetPersonByPersonID(personID);

            if (person == null)
                return null;

            //Convert matching person object from "Person" to "PersonResponse" type. 
            PersonResponse personResponse = person.ToPersonResponse();

            //Return PersonResponse object 
            return personResponse;
        }

        /// <summary>
        /// Retrieves filtered persons based on the specified search criteria.
        /// </summary>
        /// <param name="searchBy">The field to search by.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>A list of filtered PersonResponse objects.</returns>
        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPersons: {searchBy} {searchString}", searchBy?.Replace(Environment.NewLine, "") , searchString?.Replace(Environment.NewLine, "") );
            List<Person> persons = null;

            using (Operation.Time("Time for filtered persons database"))
            {
                int esNumero;
                persons = searchBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.PersonName.Contains(searchString ?? "")),

                    nameof(PersonResponse.Email) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Email.Contains(searchString ?? "")),

                    nameof(PersonResponse.DateOfBirth) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.DateOfBirth.Value.ToString("dd-MM-yyyy") == searchString),

                    nameof(PersonResponse.Gender) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Gender.Contains(searchString ?? "")),

                    nameof(PersonResponse.Country) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Country.CountryName.Contains(searchString ?? "")),

                    nameof(PersonResponse.Address) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                    temp.Address.Contains(searchString ?? "")),

                    nameof(PersonResponse.Age) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                    //Calcula la edad a partir de birthdate, si la edad es mayor o igual que searchString
                    //devuelve true
                    int.TryParse(searchString, out esNumero) &&
                    temp.DateOfBirth.HasValue &&
                    (DateTime.Now.Year - temp.DateOfBirth.Value.Year) >= int.Parse(searchString)),

                    _ => await _personsRepository.GetAllPersons()
                };
            }
            _diagnosticContext.Set("persons", persons);

            return persons.Select(temp => temp.ToPersonResponse()).ToList();

        }

        /// <summary>
        /// Retrieves all persons in CSV format.
        /// </summary>
        /// <returns>A MemoryStream containing the CSV data.</returns>
        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);

            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);

            csvConfig.Delimiter = ";"; // Establecer el separador como punto y coma


            CsvWriter csvw = new CsvWriter(sw, csvConfig, leaveOpen: true);


            //csvw.WriteHeader<PersonResponse>();
            csvw.WriteField(nameof(PersonResponse.PersonName));
            csvw.WriteField(nameof(PersonResponse.Email));
            csvw.WriteField(nameof(PersonResponse.DateOfBirth));
            csvw.WriteField(nameof(PersonResponse.Country));
            csvw.WriteField(nameof(PersonResponse.Address));
            csvw.WriteField(nameof(PersonResponse.ReceiveNewsLetter));
            csvw.NextRecord();

            List<PersonResponse> persons = await GetAllPersons();

            //await csvw.WriteRecordsAsync(persons);

            foreach (PersonResponse p in persons)
            {
                csvw.WriteField(p.PersonName);
                csvw.WriteField(p.Email);
                if (p.DateOfBirth.HasValue)
                    csvw.WriteField(p.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                else
                    csvw.WriteField(string.Empty);
                csvw.WriteField(p.Country);
                csvw.WriteField(p.Address);
                csvw.WriteField(p.ReceiveNewsLetter);

                csvw.NextRecord();
                csvw.Flush();
            }


            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Retrieves all persons in Excel format.
        /// </summary>
        /// <returns>A MemoryStream containing the Excel data.</returns>
        public virtual async Task<MemoryStream> GetPersonsEXCEL()
        {
            MemoryStream ms = new MemoryStream();

            using (ExcelPackage xls = new ExcelPackage(ms))
            {
                ExcelWorksheet ws = xls.Workbook.Worksheets.Add("Persons");
                ws.Cells["A1"].Value = "Person Name";
                ws.Cells["B1"].Value = "Email";
                ws.Cells["C1"].Value = "Date of Birth";
                ws.Cells["D1"].Value = "Age";
                ws.Cells["E1"].Value = "Gender";
                ws.Cells["F1"].Value = "Country";
                ws.Cells["G1"].Value = "Address";

                using (ExcelRange h = ws.Cells["A1:H1"])
                {
                    h.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    h.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    h.Style.Font.Bold = true;
                }


                int row = 2;

                List<PersonResponse> persons = await GetAllPersons();

                foreach (PersonResponse p in persons)
                {
                    ws.Cells[row, 1].Value = p.PersonName;
                    ws.Cells[row, 2].Value = p.Email;
                    ws.Cells[row, 3].Value = p.DateOfBirth.HasValue ? p.DateOfBirth.Value.ToString("yyyy-MM-dd") : "";
                    ws.Cells[row, 4].Value = p.Age;
                    ws.Cells[row, 5].Value = p.Gender;
                    ws.Cells[row, 6].Value = p.Country;
                    ws.Cells[row, 7].Value = p.Address;
                    row++;
                }

                ws.Cells[$"A1:H{row}"].AutoFitColumns();

                await xls.SaveAsync();
                ms.Position = 0;
            }

            return ms;
        }
    }
}
