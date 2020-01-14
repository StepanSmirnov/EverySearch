using EverySearch.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EverySearch.Lib
{
    public abstract class SearchProvider
    {

        public SearchProvider(IConfiguration configuration)
        {
            InitializeCredentials(configuration);
        }

        public IEnumerable<SearchResult> ExecuteQuery(string query, int? count)
        {
            HttpWebRequest request = MakeRequest(query, count);

            string html = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return ParseResponse(html);
        }

        protected abstract void InitializeCredentials(IConfiguration configuration);

        public abstract HttpWebRequest MakeRequest(string query, int? count);

        public abstract IEnumerable<SearchResult> ParseResponse(string result);
    }
}
