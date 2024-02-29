using Entities;
using ServiceContracts.Enums;
using System;
using System.Text;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        #region properties
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        public double? Age { get; set; }
        #endregion properties

        /// <summary>
        ///It compares the current object with another object of the same type
        ///returns true if both values are equal; otherwise false
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }

            PersonResponse person = (PersonResponse)obj;

            return this.PersonID == person.PersonID
                && this.PersonName == person.PersonName
                && this.Email == person.Email
                && this.DateOfBirth == person.DateOfBirth
                && this.Gender == person.Gender
                && this.CountryID == person.CountryID
                && this.Country == person.Country
                && this.Address == person.Address
                && this.ReceiveNewsLetter == person.ReceiveNewsLetter
                && this.Age == person.Age;
        }

        /// <summary>
        /// Overrides the GetHashCode method.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overrides the ToString method.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PersonID: ").Append(PersonID).AppendLine();
            sb.Append("PersonName: ").Append(PersonName).AppendLine();
            sb.Append("Email: ").Append(Email).AppendLine();
            sb.Append("DateOfBirth: ").Append(DateOfBirth?.ToString("MM/dd/yyyy")).AppendLine();
            sb.Append("Gender: ").Append(Gender).AppendLine();
            sb.Append("CountryID: ").Append(CountryID).AppendLine();
            sb.Append("Country: ").Append(Country).AppendLine();
            sb.Append("Address: ").Append(Address).AppendLine();
            sb.Append("ReceiveNewsLetter: ").Append(ReceiveNewsLetter).AppendLine();
            sb.Append("Age: ").AppendLine(Age?.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Converts the PersonResponse object to a PersonUpdateRequest object.
        /// </summary>
        /// <returns>A new instance of PersonUpdateRequest.</returns>
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }
    }


    public static class PersonExtensions
    {

        /// <summary>
        /// Extension method to convert Person to PersonResponse
        /// </summary>
        /// <param name="person">The person to convert</param>
        /// <returns>Person response object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {

            double? age = null;

            if (person != null)
            {
                if (person.DateOfBirth.HasValue)
                    age = Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365, 2);
            }

            GenderOptions parsedGender;
            bool isValidGender = Enum.TryParse(person.Gender, out parsedGender);


            return new PersonResponse
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = isValidGender? parsedGender: null,
                CountryID = person.CountryID,
                Country = person.Country?.CountryName,
                Address = person.Address,
                ReceiveNewsLetter = person.ReceiveNewsLetter,
                Age = age
            };
        }
    }


}
