using Microsoft.Extensions.Logging;
using SearchFight.Application.Common.Interfaces;
using SearchFight.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SearchFight.Application
{
    public class SearchEngine : ISearchEngine
    {
        private readonly ISearchContext _searchContext;
        private readonly ILogger _logger;
        public SearchEngine(ISearchContext searchContext, ILogger<SearchEngine> logger)
        {
            _searchContext = searchContext;
            _logger = logger;
        }

        public async Task<ResultDto> Search(IEnumerable<string> searchParams)
        {
            var result = new ResultDto();
            var resultSearches = new List<ResultSearchDto>();
            try
            {
                var stopWath = new Stopwatch();
                stopWath.Start();

                var searches = _searchContext.GetAllSearches();
                var tasks = new List<Task<ResultSearchDto>>();
                foreach (string searchParam in searchParams)
                {
                    foreach (var search in searches)
                    {
                        var task = search.SearchText(searchParam);
                        tasks.Add(task);
                    }
                }
                
                await Task.WhenAll(tasks);

                foreach (var task in tasks)
                {
                    if (task != null && task.Result != null)
                    {
                        resultSearches.Add(task.Result);
                    }
                }

                result.ResultBySearches = resultSearches;
                stopWath.Stop();
                _logger.LogInformation($"Time elapsed:{stopWath.ElapsedMilliseconds}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
    }
}
