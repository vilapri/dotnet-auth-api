using Microsoft.AspNetCore.Identity;
using System;

namespace Webapi.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
        }
    }
}