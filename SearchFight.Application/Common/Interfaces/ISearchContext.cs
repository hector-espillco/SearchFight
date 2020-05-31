using System.Collections.Generic;

namespace SearchFight.Application.Common.Interfaces
{
    public interface ISearchContext
    {
        IEnumerable<ISearch> GetAllSearches();
    }
}
