using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models.ViewModels
{
    public class BuyShareViewModel
    {
        public List<BrokerModel> Brokers { get; set; }
        public List<ShareModel> Shares { get; set; }
        public BrokerModel BrokerSelected { get; set; }
        public ShareModel ShareSelected { get; set; }
    }
}
