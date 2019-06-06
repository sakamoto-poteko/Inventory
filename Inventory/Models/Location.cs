using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        public string LocationName { get; set; }
        public string LocationUnit { get; set; }
        public string Comments { get; set; }

        public List<Inventory> Inventories { get; set; }
    }
}
