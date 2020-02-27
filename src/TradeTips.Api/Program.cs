using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TradeTips.Api
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        // read database configuration (database provider + database connection) 
    //        // from environment variables
    //        var config = new ConfigurationBuilder()
    //            .AddEnvironmentVariables()
    //            .Build();

    //        IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //           {
    //               webBuilder
    //                   //.UseConfiguration(config)
    //                   //.UseUrls($"http://+:5000")
    //                   .UseStartup<Startup>();
    //           });

    //        IHost host = hostBuilder.Build();

    //        host.Run();
    //    }
    //}

    public class Program
    {
        public static void Main(string[] args)
        {
            // read database configuration (database provider + database connection) from environment variables
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            // TODO Replace this with CreateDefaultBuilder
            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseUrls($"http://+:5000")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
