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
    public class GoogleSearch : ISearch
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public GoogleSearch(IHttpClientFactory clientFactory, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<GoogleSearch>();
        }

        public async Task<ResultSearchDto> SearchText(string param)
        {
            ResultSearchDto result = null;
            try
            {
                _logger.LogInformation("Searching in Google");
                long totalResult = 0;
                var apiKey = _configuration["Searches:Google:apiKey"];
                var cx = _configuration["Searches:Google:apiCustomSearch"];
                var client = _clientFactory.CreateClient("GoogleService");

                var response = await client.GetAsync($"?key={apiKey}&cx={cx}&q={Uri.EscapeDataString(param)}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);
                    long.TryParse(data?.searchInformation?.totalResults?.Value, out totalResult);
                }

                return new ResultSearchDto
                {
                    SearchType = SearchType.Google.GetValue(),
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
