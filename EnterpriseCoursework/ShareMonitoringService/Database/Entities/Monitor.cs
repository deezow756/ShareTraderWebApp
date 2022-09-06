using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShareMonitoringService.Database.Entities
{
    public class Monitor
    {
        [Key]
        public int Id { get; set; }
        public int ShareId { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Currency)]
        public double Min { get; set; }
        [DataType(DataType.Currency)]
        public double Max { get; set; }
        public bool Viewed { get; set; }
    }
}
