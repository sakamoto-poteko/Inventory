﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comments { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
