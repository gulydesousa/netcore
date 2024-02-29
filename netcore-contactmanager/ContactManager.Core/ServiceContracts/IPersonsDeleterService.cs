namespace ServiceContracts
{
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes a person based on the given PersonID
        /// </summary>
        /// <param name="personID">Person ID to delete</param>
        /// <returns>True if deletion is successful, otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);
       
    }
}
