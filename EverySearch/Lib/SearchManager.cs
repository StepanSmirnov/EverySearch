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
            List<Task<SearchResultSet>> tasks = new List<Task<SearchResultSet>>();
            foreach (var item in searchProviders)
            {
                tasks.Add(Task<SearchResultSet>.Run(() => ConcurrentExec(item, query, count)));
            }
            while (tasks.Count() > 0)
            {
                var task = await Task.WhenAny(tasks);
                if (task.Result != null)
                {
                    return task.Result;
                }
                tasks.Remove(task);
            }
            return new List<SearchResult>();
        }

        private SearchResultSet ConcurrentExec(SearchProvider provider, string query, int? count)
        {
            try
            {
                var res = provider.ExecuteQuery(query, count);
                return res;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception while executing query '{query}' by provider {provider}", query, provider.GetType().Name);
                return null;
            }
        }
    }
}
