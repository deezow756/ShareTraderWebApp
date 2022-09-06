using Microsoft.EntityFrameworkCore;
using ShareAlertService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareAlertService.Database
{
    public class ShareAlertContext : DbContext
    {
        public DbSet<ShareAlert> ShareAlerts { get; set; }

        public ShareAlertContext(DbContextOptions<ShareAlertContext> options)
            : base(options)
        {
        }
    }
}
