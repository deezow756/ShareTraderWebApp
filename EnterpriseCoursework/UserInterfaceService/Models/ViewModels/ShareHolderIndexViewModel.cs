using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models.ViewModels
{
    public class ShareHolderIndexViewModel
    {
        public List<ShareHolderModel> ShareHolders { get; set; }
        public List<ShareModel> Shares { get; set; }
        public List<IdentityUser> Users { get; set; }
        public List<BrokerModel> Brokers { get; set; }
        public List<ShareAlertModel> Alerts { get; set; }
        public List<TraderInfoModel> Trades { get; set; }
    }
}
