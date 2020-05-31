using Microsoft.Extensions.Logging;
using SearchFight.Application.Common.Interfaces;
using SearchFight.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ResultDto Search(IEnumerable<string> searchParams)
        {
            var result = new ResultDto();
            var resultSearches = new List<ResultSearchDto>();
            try
            {
                var searches = _searchContext.GetAllSearches();

                var tasks = new List<Task>();
                foreach (string searchParam in searchParams)
                {
                    foreach (var search in searches)
                    {
                        var task = search.SearchText(searchParam);
                        tasks.Add(task);
                    }
                }
                Task.WaitAll(tasks.ToArray());

                foreach (dynamic task in tasks)
                {
                    if (task.Result != null)
                    {
                        resultSearches.Add(task.Result);
                    }
                }

                result.ResultByParams = resultSearches
                        .GroupBy(s => s.SearchParam, s => s, (s, r) => new { param = s, result = r.ToList() })
                        .Select(r => new ResultParamDto
                        {
                            SearchParam = r.param,
                            ResultSearchs = r.result
                        })
                        .ToList();

                result.WinnerByTypes = resultSearches
                        .GroupBy(s => s.SearchType, s => s, (s, r) => new { type = s, result = r.OrderByDescending(a => a.Amount).First() })
                        .Select(r => new ResultSearchDto
                        {
                            SearchType = r.type,
                            Amount = r.result.Amount,
                            SearchParam = r.result.SearchParam
                        })
                        .ToList();

                result.WinnerParam = resultSearches.OrderByDescending(a => a.Amount).FirstOrDefault()?.SearchParam;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
    }
}
