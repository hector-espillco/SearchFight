using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SearchFight.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace SearchFight.Infrastructure.Data
{
    public class SearchContext : ISearchContext
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public SearchContext(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<SearchContext> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public IEnumerable<ISearch> GetAllSearches()
        {
            var searches = new List<ISearch>();
            try
            {
                var type = typeof(ISearch);
                var searchObjects = Assembly.Load(type.Assembly.GetName().Name).GetTypes()
                    .Where(t => t.IsClass && t.GetTypeInfo().ImplementedInterfaces.Any(i => i == type))
                    .ToList();

                foreach (var searchObject in searchObjects)
                {
                    var search = (ISearch)Activator.CreateInstance(searchObject, new object[] { _clientFactory, _configuration });
                    searches.Add(search);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return searches;
        }
    }
}
