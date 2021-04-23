using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Models
{
    public class LoginResponse
    {
        public Guid ApplicationUserId { get; set; }
        public bool Success { get; set; }
        public List<string> UserRoles { get; set; }
    }
}