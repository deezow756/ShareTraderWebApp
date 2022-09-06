using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShareService.Database.Entities;

namespace ShareService.Database
{
    public class ShareContext : DbContext
    {
        public DbSet<Share> Shares { get; set; }

        public ShareContext(DbContextOptions<ShareContext> options)
            : base(options)
        {
        }
    }
}
