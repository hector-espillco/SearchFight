using Microsoft.Extensions.Configuration;
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

        public BingSearch(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<ResultSearchDto> SearchText(string param)
        {
            try
            {
                long totalResult = 0;
                var client = _clientFactory.CreateClient("BingService");
                var response = await client.GetAsync($"?q={Uri.EscapeDataString(param)}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(content);
                    long.TryParse(result?.webPages?.totalEstimatedMatches?.Value?.ToString(), out totalResult);
                }

                return new ResultSearchDto
                {
                    SearchType = SearchType.Bing.GetValue(),
                    SearchParam = param,
                    Amount = totalResult
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
