using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TraderInfoService.Database.Entities
{
    public class TraderInfo
    {
        [Key]
        public int Id { get; set; }
        public int TradingCode { get; set; }
        [DataType(DataType.Date)]
        public DateTime TradeDate { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public int Amount { get; set; }
        public string BuyerId { get; set; }
        public int SellerId { get; set; }
    }
}
