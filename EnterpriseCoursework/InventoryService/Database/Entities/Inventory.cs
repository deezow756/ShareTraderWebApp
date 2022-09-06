using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Database.Entities
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Item name must be at least 5 characters")]
        [StringLength(30, MinimumLength = 5)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Item description must be at least 5 characters")]
        [StringLength(250, MinimumLength = 5)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Must enter a valid quantity")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Must enter a valid price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public bool Threefortwo { get; set; }

        public bool Bogof { get; set; }
        public bool FreeDelivery { get; set; }
    }
}
