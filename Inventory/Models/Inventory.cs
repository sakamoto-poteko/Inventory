using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Documents;

namespace Inventory.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        public Product Product { get; set; }

        public int LocationId { get; set; }

        [Required]
        public Location Location { get; set; }

        public int Quantity { get; set; }

        public string Comments { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
