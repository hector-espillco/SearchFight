using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SearchFight.Application.Common.Interfaces;
using SearchFight.Application.Dtos;
using SearchFight.Application.Helper;
using SearchFight.Domain.Enum;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchFight.Application.Searches
{
    public class BingSearch : ISearch
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public BingSearch(IHttpClientFactory clientFactory, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<BingSearch>();
        }
        public async Task<ResultSearchDto> SearchText(string param)
        {
            ResultSearchDto result = null;
            try
            {
                _logger.LogInformation("Searching in Bing new");
                long totalResult = 0;
                var client = _clientFactory.CreateClient("BingService");
                var response = await client.GetAsync($"?q={Uri.EscapeDataString(param)}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);
                    long.TryParse(data?.webPages?.totalEstimatedMatches?.Value?.ToString(), out totalResult);
                }

                result = new ResultSearchDto
                {
                    SearchType = SearchType.Bing.GetValue(),
                    SearchParam = param,
                    Total = totalResult
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
    }
}
