using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models.ViewModels
{
    public class InterestedViewModel
    {
        public List<MonitorModel> Monitors { get; set; }
        public List<ShareModel> Shares { get; set; }
    }
}
