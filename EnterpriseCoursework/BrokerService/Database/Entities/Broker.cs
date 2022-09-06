using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrokerService.Database.Entities
{
    public class Broker
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Postcode { get; set; }
        public string Domain { get; set; }
        public string TradingRecord { get; set; }
        public string ServiceQualityGrade { get; set; }
    }
}
