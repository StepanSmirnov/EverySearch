using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EverySearch.Models
{
    public class Search
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Query { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }

        public virtual ICollection<SearchResult> SearchResults { get; set; }
    }
}
