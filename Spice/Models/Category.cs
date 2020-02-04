using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Category")]
        [Required]
        public string Name { get; set; }

        // *** NOTE: Don't forget to add to ApplicationDbContext as 
        //  public DbSet<Category> Category { get; set; } for migration purposes
    }
}
