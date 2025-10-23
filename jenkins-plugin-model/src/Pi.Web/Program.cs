using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pi.Math;
using Pi.Runtime;
using PowerArgs;
using Prometheus;
using System;
using System.IO;

namespace Pi.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var arguments = Args.Parse<Arguments>(args);
                switch (arguments.Mode)
                {
                    case RunMode.Web:
                        Console.WriteLine("Running in web mode...");
                        var builder = WebApplication.CreateBuilder(args);
                        
                        // Add services to the container
                        builder.Services.AddControllersWithViews();
                        
                        var app = builder.Build();
                        
                        // Configure the HTTP request pipeline
                        if (bool.Parse(app.Configuration["Computation:Metrics:Enabled"] ?? "false"))
                        {
                            app.UseMetricServer();
                            app.UseHttpMetrics();
                        }

                        if (app.Environment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }
                        else
                        {
                            app.UseExceptionHandler("/Home/Error");
                        }

                        app.UseStaticFiles();
                        app.UseRouting();

                        app.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Pi}/{action=Index}/{id?}");
                        
                        app.Run();
                        break;

                    case RunMode.Console:
                        Console.WriteLine(GetPi(arguments.DecimalPlaces));
                        break;

                    case RunMode.File:
                        File.WriteAllText(arguments.OutputPath, GetPi(arguments.DecimalPlaces));
                        Console.WriteLine($"Wrote pi to: {arguments.DecimalPlaces} dp; at: {arguments.OutputPath}");
                        break;
                }
            }
            catch (ArgException)
            {
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<Arguments>());
            }
        }

        private static string GetPi(int decimalPlaces)
        {
            return MachinFormula.Calculate(decimalPlaces).ToString();
        }
    }
}
