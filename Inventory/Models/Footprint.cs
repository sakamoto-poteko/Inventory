using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Footprint
    {
        [Key]
        public int FootprintId { get; set; }

        [Required]
        public string FootprintName { get; set; }

        public string Comments { get; set; }

        public List<Product> Products { get; set; }
    }
}
