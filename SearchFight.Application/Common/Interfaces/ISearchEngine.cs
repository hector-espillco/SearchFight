using SearchFight.Application.Dtos;
using System.Collections.Generic;

namespace SearchFight.Application.Common.Interfaces
{
    public interface ISearchEngine
    {
        ResultDto Search(IEnumerable<string> searchParams);
    }
}
