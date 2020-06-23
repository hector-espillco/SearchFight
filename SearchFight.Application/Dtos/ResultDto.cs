using System.Collections.Generic;

namespace SearchFight.Application.Dtos
{
    public class ResultDto
    {
        public ResultDto()
        {
            ResultBySearches = new HashSet<ResultSearchDto>();
        }
        public IEnumerable<ResultSearchDto> ResultBySearches { get; set; }
    }
}
