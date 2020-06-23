using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SearchFight.Application.Common.Interfaces;
using SearchFight.Application.Searches;
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
        private readonly ILoggerFactory _loggerFactory;
        public SearchContext(IHttpClientFactory clientFactory, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<SearchContext>();
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
                    _logger.LogInformation("Creating search");
                    var search = (ISearch)Activator.CreateInstance(searchObject, new object[] { _clientFactory, _configuration, _loggerFactory });
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
