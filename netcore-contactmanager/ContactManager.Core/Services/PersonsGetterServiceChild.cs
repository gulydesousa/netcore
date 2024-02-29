using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;

namespace Services
{
    /// <summary>
    /// Represents a child class of PersonsGetterService.
    /// </summary>
    public class PersonsGetterServiceChild : PersonsGetterService
    {
        /// <summary>
        /// Initializes a new instance of the PersonsGetterServiceChild class.
        /// </summary>
        /// <param name="personsRepository">The persons repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="_diagnosticContext">The diagnostic context.</param>
        public PersonsGetterServiceChild(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext _diagnosticContext)
            : base(personsRepository, logger, _diagnosticContext)
        {
        }

        /// <summary>
        /// Gets the persons data in Excel format.
        /// </summary>
        /// <returns>A MemoryStream containing the Excel data.</returns>
        public override async Task<MemoryStream> GetPersonsEXCEL()
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
