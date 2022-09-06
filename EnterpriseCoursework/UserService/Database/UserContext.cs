using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Database
{
    public class UserContext : IdentityDbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            
        }
    }
}
