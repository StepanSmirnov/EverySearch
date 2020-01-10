using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverySearch.Models
{
    public class EverySearchContext: DbContext
    {
        public EverySearchContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Search> Searches { get; set; }
        
        public DbSet<SearchResult> SearchResults { get; set; }
    }
}
