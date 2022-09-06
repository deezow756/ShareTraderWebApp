using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserInterfaceService.Models;

namespace UserInterfaceService.Data
{
    public class UserInterfaceServiceContext : DbContext
    {
        public UserInterfaceServiceContext (DbContextOptions<UserInterfaceServiceContext> options)
            : base(options)
        {
        }

        public DbSet<UserInterfaceService.Models.ShareModel> ShareModel { get; set; }

        public DbSet<UserInterfaceService.Models.MonitorModel> MonitorModel { get; set; }

        public DbSet<UserInterfaceService.Models.TraderInfoModel> TraderInfoModel { get; set; }

        public DbSet<UserInterfaceService.Models.BrokerModel> BrokerModel { get; set; }

        public DbSet<UserInterfaceService.Models.ShareAlertModel> ShareAlertModel { get; set; }
    }
}
