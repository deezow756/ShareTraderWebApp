using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models.ViewModels
{
    public class CreateAlertViewModel
    {
        public ShareAlertModel ShareAlert { get; set; }
        public List<ShareModel> Shares { get; set; }
    }
}
