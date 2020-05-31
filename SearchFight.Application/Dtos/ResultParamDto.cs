using System.Collections.Generic;

namespace SearchFight.Application.Dtos
{
    public class ResultParamDto
    {
        public ResultParamDto()
        {
            ResultSearchs = new HashSet<ResultSearchDto>();
        }
        public string SearchParam { get; set; }
        public IEnumerable<ResultSearchDto> ResultSearchs { get; set; }
    }
}
