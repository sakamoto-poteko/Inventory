using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public int? FootprintId { get; set; }
        public Footprint Footprint { get; set; }

        public string Manufacturer { get; set; }

        public string Comments { get; set; }

        public List<Inventory> Inventories { get; set; }
    }
}
