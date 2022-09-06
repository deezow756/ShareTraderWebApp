using Microsoft.EntityFrameworkCore;
using ShareMonitoringService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareMonitoringService.Database
{
    public class MonitorContext : DbContext
    {
        public DbSet<Monitor> Monitors { get; set; }

        public MonitorContext(DbContextOptions<MonitorContext> options)
            : base(options)
        {
        }

    }
}
