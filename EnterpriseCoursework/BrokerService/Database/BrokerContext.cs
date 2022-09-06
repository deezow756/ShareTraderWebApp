using BrokerService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrokerService.Database
{
    public class BrokerContext : DbContext
    {
        public DbSet<Broker> Brokers { get; set; }

        public BrokerContext(DbContextOptions<BrokerContext> options)
            : base(options)
        {
        }
    }
}
