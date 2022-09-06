using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class ShareModel
    {        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int TradingCode { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public double CompanyMarketValue { get; set; }
        public int BrokerId { get; set; }

        public bool Selected = false;
    }
}
