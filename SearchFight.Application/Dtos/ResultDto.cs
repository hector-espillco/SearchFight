using System.Collections.Generic;

namespace SearchFight.Application.Dtos
{
    public class ResultDto
    {
        public ResultDto()
        {
            ResultByParams = new HashSet<ResultParamDto>();
            WinnerByTypes = new HashSet<ResultSearchDto>();
        }
        public IEnumerable<ResultParamDto> ResultByParams { get; set; }

        public IEnumerable<ResultSearchDto> WinnerByTypes { get; set; }

        public string WinnerParam { get; set; }
    }
}
