using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class MonitorModel
    {
        public int Id { get; set; }
        public int ShareId { get; set; }
        public string UserId { get; set; }
        [Required]
        public double Min { get; set; }
        [Required]
        public double Max { get; set; }
        [Required]
        public double CurValue { get; set; }
        public bool Viewed { get; set; }
    }
}
