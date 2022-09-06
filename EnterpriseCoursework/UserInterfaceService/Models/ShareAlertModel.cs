using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class ShareAlertModel
    {
        public int Id { get; set; }
        public int ShareId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
