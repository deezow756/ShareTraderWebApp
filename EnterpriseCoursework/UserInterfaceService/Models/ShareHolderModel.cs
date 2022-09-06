using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class ShareHolderModel
    {        
        public int Id { get; set; }
        public int ShareId { get; set; }
        public string UserId { get; set; }
        public int BrokerId { get; set; }
    }
}
