using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Price_Calculator_Kata.Models;
using System;
using System.IO;

namespace Price_Calculator_Kata
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<App>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();
            services.AddSingleton(config);

            services.AddTransient<IProductDetailsManager, ProductDetailsManager>();
            services.AddTransient<IPriceCalculatorStringBuilder, PriceCalculatorStringBuilder>();
            services.AddTransient<IPriceCalulatorManager, PriceCalulatorManager>();
            services.AddTransient<IConfigurationWrapper, ConfigurationWrapper>();

            services.AddTransient<App>();

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
