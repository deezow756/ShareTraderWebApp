using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShareAlertService.Database.Entities
{
    public class ShareAlert
    {
        [Key]
        public int Id { get; set; }
        public int ShareId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
