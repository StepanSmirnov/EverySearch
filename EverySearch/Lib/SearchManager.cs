using EverySearch.Models;
using EverySearch.Lib;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EverySearch.Lib
{
    public class SearchManager
    {
        private List<SearchProvider> searchProviders;
        private int isExecuted = 0;

        public SearchManager(IConfiguration configuration)
        {
            searchProviders = new List<SearchProvider>
            {
                new GoogleProvider(configuration)
            };
        }

        public void ExecuteQuery(string query, Action<IEnumerable<SearchResult>> onSuccess, Action<string> onError)
        {
            isExecuted = 0;
            Task<IEnumerable<SearchResult>>[] tasks = new Task<IEnumerable<SearchResult>>[searchProviders.Count];
            foreach (var item in searchProviders)
            {
                tasks.Append(Task.Run(() => ConcurrentExec(item, query, onSuccess)));
            }
            Task.WaitAll(tasks);
            if (isExecuted == 0)
            {
                onError("fatal error");
            }
        }

        private void ConcurrentExec(SearchProvider provider, string query, Action<IEnumerable<SearchResult>> onSuccess)
        {
            try
            {
                IEnumerable<SearchResult>  result = provider.ExecuteQuery(query);
                Interlocked.CompareExchange(ref isExecuted, 1, 0);
                if (isExecuted == 1)
                {
                    onSuccess(result);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
