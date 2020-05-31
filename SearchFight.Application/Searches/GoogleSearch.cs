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
    public class GoogleSearch : ISearch
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public GoogleSearch(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<ResultSearchDto> SearchText(string param)
        {
            try
            {
                long totalResult = 0;
                var apiKey = _configuration["Searches:Google:apiKey"];
                var cx = _configuration["Searches:Google:apiCustomSearch"];
                var client = _clientFactory.CreateClient("GoogleService");

                var response = await client.GetAsync($"?key={apiKey}&cx={cx}&q={Uri.EscapeDataString(param)}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(content);
                    long.TryParse(result?.searchInformation?.totalResults?.Value, out totalResult);
                }

                return new ResultSearchDto
                {
                    SearchType = SearchType.Google.GetValue(),
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
