using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class UserInterfaceContext : IdentityDbContext
    {
        public UserInterfaceContext(DbContextOptions<UserInterfaceContext> options)
            : base(options)
        {
        }
    }
}
