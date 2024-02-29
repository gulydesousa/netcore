using Microsoft.AspNetCore.Identity;


namespace ContactManager.Core.Domain.IdentityEntites
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

    }
}
