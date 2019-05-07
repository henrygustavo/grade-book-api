﻿namespace GradeBook.Api
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Events;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Getting the Identity API running...");

                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Identity API  terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, builder) =>
             {
                 IHostingEnvironment env = context.HostingEnvironment;
                 builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                    AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                                                                      reloadOnChange: true)
                    .AddEnvironmentVariables();
             })
                 .UseStartup<Startup>()
                 .UseSerilog()
                 .Build();
    }
}
