using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Models
{
    public class Transaction
    {
        public enum InventoryDirection
        {
            Addition,
            Removal,
            Shrinkage
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public Inventory Inventory { get; set; }

        public InventoryDirection Direction { get; set; }

        public Supplier Supplier { get; set; }

        public int Quantity { get; set; }

        public int? Price { get; set; }

        public DateTime Time { get; set; }

        public string Comments { get; set; }
    }
}
