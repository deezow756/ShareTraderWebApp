using Microsoft.EntityFrameworkCore;
using ShareHolderService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareHolderService.Database
{
    public class ShareHolderContext : DbContext
    {
        public DbSet<ShareHolder> ShareHolders { get; set; }

        public ShareHolderContext(DbContextOptions<ShareHolderContext> options)
            : base(options)
        {
        }
    }
}
