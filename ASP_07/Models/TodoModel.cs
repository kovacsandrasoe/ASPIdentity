using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_07.Models
{
    public class TodoModel
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string OwnerId { get; set; }
    }
}
