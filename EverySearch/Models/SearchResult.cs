using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EverySearch.Models
{
    public class SearchResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Url { get; set; }

        [Required]
        public string DisplayUrl { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Snippet { get; set; }

        [Required]
        public virtual Search Search { get; set; }
    }
}
