using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class BrokerModel
    {        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address
        {
            get
            {
                string temp = "";
                temp += Address1 + ", ";
                if(!string.IsNullOrEmpty(Address2))
                { temp += Address2 + ", "; }
                if(!string.IsNullOrEmpty(Address3))
                { temp += Address3 + ", "; }
                temp += Postcode;
                return temp;
            }
        }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Domain { get; set; }
        public string TradingRecord { get; set; }
        [Required]
        public string ServiceQualityGrade { get; set; }

        public bool Selected = false;
    }
}
