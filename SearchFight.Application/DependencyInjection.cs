using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchFight.Application.Common.Interfaces;
using SearchFight.Application.Searches;
using System;

namespace SearchFight.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISearchEngine, SearchEngine>();
            services.AddScoped<ISearch, GoogleSearch>();
            services.AddScoped<ISearch, BingSearch>();

            services.AddHttpClient("GoogleService", config =>
            {
                config.BaseAddress = new Uri(configuration["Searches:Google:apiUrl"]);
            });

            services.AddHttpClient("BingService", config =>
            {
                config.BaseAddress = new Uri(configuration["Searches:Bing:apiUrl"]);
                config.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configuration["Searches:Bing:apiKey"]);
            });

            return services;
        }
    }
}
