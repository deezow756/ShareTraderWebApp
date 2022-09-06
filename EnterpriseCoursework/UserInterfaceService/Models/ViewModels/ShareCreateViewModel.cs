using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models.ViewModels
{
    public class ShareCreateViewModel
    {
        public ShareModel Share { get; set; }
        public List<BrokerModel> Brokers { get; set; }
    }
}
