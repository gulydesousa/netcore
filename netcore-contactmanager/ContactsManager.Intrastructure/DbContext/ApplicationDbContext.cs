using ContactManager.Core.Domain.IdentityEntites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Entities
{
    /// <summary>
    /// Represents the application database context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for the database context.</param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        /// <summary>
        /// Gets or sets the DbSet for the countries.
        /// </summary>
        public virtual DbSet<Country> Countries { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for the persons.
        /// </summary>
        public virtual DbSet<Person> Persons { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to countries
            string countriesJson = File.ReadAllText("countries.json");
            if (!string.IsNullOrEmpty(countriesJson))
            {
                List<Country> countries = JsonSerializer.Deserialize<Country[]>(countriesJson)!.ToList<Country>();
                modelBuilder.Entity<Country>().HasData(countries);
            }

            //Seed to persons
            string personsJson = File.ReadAllText("persons.json");
            if (!string.IsNullOrEmpty(personsJson))
            {
                List<Person> persons = JsonSerializer.Deserialize<Person[]>(personsJson)!.ToList<Person>();
                modelBuilder.Entity<Person>().HasData(persons);
            }

            //Fluent API
            modelBuilder.Entity<Person>().Property(p => p.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");

            modelBuilder.Entity<Person>()
                .ToTable(c => c.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));

            //Relaciones entre tablas
            modelBuilder.Entity<Person>(
                entity =>
                {
                    entity.HasOne<Country>(c => c.Country)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(p => p.CountryID);
                });
        }

        /// <summary>
        /// Executes the stored procedure to get all persons.
        /// </summary>
        /// <returns>A list of persons.</returns>
        public async Task<List<Person>> sp_PersonsGetAll()
        {
            return await Persons.FromSqlRaw("EXECUTE [dbo].[Persons_GetAll];").ToListAsync();
        }

        /// <summary>
        /// Executes the stored procedure to insert a person.
        /// </summary>
        /// <param name="person">The person to insert.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> sp_PersonsInsert(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@PersonID", person.PersonID),
                    new SqlParameter("@PersonName", person.PersonName),
                    new SqlParameter("@Email", person.Email),
                    new SqlParameter("@DateOfBirth", person.DateOfBirth),
                    new SqlParameter("@Gender", person.Gender),
                    new SqlParameter("@CountryID", person.CountryID),
                    new SqlParameter("@Address", person.Address),
                    new SqlParameter("@ReceiveNewsLetter", person.ReceiveNewsLetter)
            };

            return await
            Database.ExecuteSqlRawAsync("EXEC [dbo].[Persons_Insert] " +
            "@PersonID , @PersonName, @Email, @DateOfBirth, @Gender, " +
            "@CountryID, @Address, @ReceiveNewsLetter", parameters);
        }
    }
}
