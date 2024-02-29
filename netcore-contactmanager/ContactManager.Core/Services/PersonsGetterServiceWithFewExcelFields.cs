using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    /// <summary>
    /// Service class for getting persons with few Excel fields.
    /// </summary>
    public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
    {
        private readonly PersonsGetterService _personsGetterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsGetterServiceWithFewExcelFields"/> class.
        /// </summary>
        /// <param name="personsGetterService">The persons getter service.</param>
        public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }

        /// <inheritdoc/>
        public Task<List<PersonResponse>> GetAllPersons()
        {
            return _personsGetterService.GetAllPersons();
        }

        /// <inheritdoc/>
        public Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            return _personsGetterService.GetFilteredPersons(searchBy, searchString);
        }

        /// <inheritdoc/>
        public Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            return _personsGetterService.GetPersonByPersonID(personID);
        }

        /// <inheritdoc/>
        public Task<MemoryStream> GetPersonsCSV()
        {
            return _personsGetterService.GetPersonsCSV();
        }

        /// <inheritdoc/>
        public async Task<MemoryStream> GetPersonsEXCEL()
        {
            MemoryStream ms = new MemoryStream();

            using (ExcelPackage xls = new ExcelPackage(ms))
            {
                ExcelWorksheet ws = xls.Workbook.Worksheets.Add("Persons");
                ws.Cells["A1"].Value = "Person Name";
                ws.Cells["B1"].Value = "Email";
                //ws.Cells["C1"].Value = "Date of Birth";
                //ws.Cells["D1"].Value = "Age";
                //ws.Cells["E1"].Value = "Gender";
                ws.Cells["C1"].Value = "Country";
                ws.Cells["D1"].Value = "Address";

                using (ExcelRange h = ws.Cells["A1:D1"])
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
                    //ws.Cells[row, 3].Value = p.DateOfBirth.HasValue ? p.DateOfBirth.Value.ToString("yyyy-MM-dd") : "";
                    //ws.Cells[row, 4].Value = p.Age;
                    //ws.Cells[row, 5].Value = p.Gender;
                    ws.Cells[row, 3].Value = p.Country;
                    ws.Cells[row, 4].Value = p.Address;
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
