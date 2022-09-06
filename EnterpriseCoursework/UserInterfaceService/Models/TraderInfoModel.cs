using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class TraderInfoModel
    {        
        public int Id { get; set; }
        [Required]
        public int TradingCode { get; set; }
        [Required]
        public DateTime TradeDate { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string BuyerId { get; set; }
        [Required]
        public int SellerId { get; set; }
        public bool Selected = false;
    }
}
