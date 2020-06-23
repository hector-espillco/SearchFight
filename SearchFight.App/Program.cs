using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchFight.Application;
using SearchFight.Application.Common.Interfaces;
using SearchFight.Infrastructure;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SearchFight.App
{
    class Program
    {
        static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            RegisterServices();
            Process(args);
            DisposeServices();
        }

        static void RegisterServices()
        {
            var services = new ServiceCollection();

            var configuration = LoadConfiguration();
            services.AddSingleton(configuration);

            Log.Logger = LoadLog();
            services.AddLogging(configure => configure.AddSerilog());

            services.AddInfrastructure();
            services.AddApplication(configuration);

            _serviceProvider = services.BuildServiceProvider();
        }

        static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.json"), optional: true);

            return builder.Build();
        }

        static ILogger LoadLog()
        {
            return new LoggerConfiguration()
                .WriteTo.File(Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"Logs/SearchFight-{DateTime.Now:yyyyMMdd}.txt"))
                .CreateLogger();
        }

        static void Process(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            var searchEngine = _serviceProvider.GetService<ISearchEngine>();
            var result = searchEngine.Search(args.ToList())?.Result;
            if (result == null || result.ResultBySearches == null || result.ResultBySearches.Any() == false)
                return;

            var resultByParams = result.ResultBySearches
                                .GroupBy(s => s.SearchParam, s => s, (s, r) => new { param = s, result = r.ToList() })
                                .Select(r => new 
                                {
                                    SearchParam = r.param,
                                    ResultSearchs = r.result
                                })
                                .ToList();

            var sb = new StringBuilder();
            foreach (var resultParam in resultByParams)
            {
                sb.Append($"{resultParam.SearchParam}: ");
                foreach (var resultSearch in resultParam.ResultSearchs)
                {
                    sb.Append($"{resultSearch.SearchType}: {resultSearch.Total} ");
                }
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }

            var winnerByTypes = result.ResultBySearches
                                .GroupBy(s => s.SearchType, s => s, (s, r) => new { type = s, result = r.OrderByDescending(a => a.Total).First() })
                                .Select(r => new 
                                {
                                    SearchType = r.type,
                                    r.result.Total,
                                    r.result.SearchParam
                                })
                                .ToList();

            foreach (var winnerParam in winnerByTypes)
            {
                Console.WriteLine($"{winnerParam.SearchType} winner:{winnerParam.SearchParam}");
            }

            var winner = result.ResultBySearches.OrderByDescending(a => a.Total).FirstOrDefault()?.SearchParam;
            Console.WriteLine($"Total winner:{winner}");

        }

        static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
