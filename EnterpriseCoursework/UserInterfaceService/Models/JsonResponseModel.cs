using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterfaceService.Models
{
    public class JsonResponseModel
    {
        public bool status { get; set; }
        public string msg { get; set; }
        public string[] msgs { get; set; }
    }
}
