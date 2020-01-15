using EverySearch.Models;
using EverySearch.Lib;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchResultSet = System.Collections.Generic.IEnumerable<EverySearch.Models.SearchResult>;

namespace EverySearch.Lib
{
    public class SearchManager
    {
        private readonly ILogger<SearchManager> logger;
        private List<SearchProvider> searchProviders;

        public SearchManager(IConfiguration configuration, ILogger<SearchManager> logger)
        {
            searchProviders = new List<SearchProvider>
            {
                new GoogleProvider(configuration),
                new YandexProvider(configuration),
                new BingProvider(configuration)
            };
            this.logger = logger;
        }
        public async Task<SearchResultSet> ExecuteQueryAsync(string query, int? count = null)
        {
            List<Func<SearchResultSet>> funcs = new List<Func<SearchResultSet>>();
            foreach (var provider in searchProviders)
            {
                funcs.Add(() => provider.ExecuteQuery(query, count));
            }
            var result = await Utils.WaitAnySuccessful(funcs);
            if (result.Value == null)
            {
                foreach (var exception in result.Exceptions)
                {
                    logger.LogError(exception, "Exception while executing query '{query}'. Stacktrace: {StackTrace}", query, exception.StackTrace);
                }
                return new List<SearchResult>();
            }
            return result.Value;
        }
    }
}
