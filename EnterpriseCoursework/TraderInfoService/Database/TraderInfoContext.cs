using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderInfoService.Database.Entities;

namespace TraderInfoService.Database
{
    public class TraderInfoContext : DbContext
    {
        public DbSet<TraderInfo> TraderInfos { get; set; }

        public TraderInfoContext(DbContextOptions<TraderInfoContext> options)
            : base(options)
        {
        }
    }
}
