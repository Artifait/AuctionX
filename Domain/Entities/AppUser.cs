using Microsoft.AspNetCore.Identity;

namespace AucX.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public required string FullName { get ;set; }
    }
}
