using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShareService.Database.Entities
{
    public class Share
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Trading code must be entered")]
        public int TradingCode { get; set; }
        [Required(ErrorMessage = "Must enter a valid quantity")]
        public string Quantity { get; set; }
        
        [Required(ErrorMessage = "Must enter a valid price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Item description must be at least 5 characters")]
        [StringLength(30, MinimumLength = 5)]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Must enter a valid company market value")]
        [DataType(DataType.Currency)]
        public double CompanyMarketValue { get; set; }
        public int BrokerId { get; set; }
    }
}
