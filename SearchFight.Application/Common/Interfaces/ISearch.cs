using SearchFight.Application.Dtos;
using System.Threading.Tasks;

namespace SearchFight.Application.Common.Interfaces
{
    public interface ISearch
    {
        Task<ResultSearchDto> SearchText(string param);
    }
}
