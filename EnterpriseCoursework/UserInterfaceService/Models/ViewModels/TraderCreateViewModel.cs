using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models.ViewModels
{
    public class TraderCreateViewModel
    {
        public TraderInfoModel Trader { get; set; }
        public List<ShareModel> Shares { get; set; }
        public List<IdentityUser> Users { get; set; }
        public List<BrokerModel> Brokers { get; set; }
    }
}
