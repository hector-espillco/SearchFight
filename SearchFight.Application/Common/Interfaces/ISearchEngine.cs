using SearchFight.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchFight.Application.Common.Interfaces
{
    public interface ISearchEngine
    {
        Task<ResultDto> Search(IEnumerable<string> searchParams);
    }
}
