using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShareHolderService.Database.Entities
{
    public class ShareHolder
    {
        [Key]
        public int Id { get; set; }
        public int ShareId { get; set; }
        public string UserId { get; set; }
        public int BrokerId { get; set; }
    }
}
