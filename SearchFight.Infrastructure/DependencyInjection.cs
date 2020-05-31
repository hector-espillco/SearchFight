using Microsoft.Extensions.DependencyInjection;

using SearchFight.Application.Common.Interfaces;
using SearchFight.Infrastructure.Data;

namespace SearchFight.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ISearchContext, SearchContext>();

            return services;
        }
    }
}
