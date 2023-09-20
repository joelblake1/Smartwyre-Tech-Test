using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    { 
        var serviceProvider = new ServiceCollection()
            .AddTransient<IRebateService, RebateService>()
            .AddTransient<IDataReader<Product>, ProductDataStore>()
            .AddTransient<IDataReader<Rebate>, RebateDataStore>()
            .AddTransient<IRebateDataWriter, RebateDataStore>()
            .AddTransient<IRebateCalculator, FixedCashAmountRebateCalculator>()
            .AddTransient<IRebateCalculator, FixedRateRebateCalculator>()
            .AddTransient<IRebateCalculator, AmountPerUomCalculator>()
            .AddSingleton<Func<IEnumerable<IRebateCalculator>>>(x=> () => x.GetService<IEnumerable<IRebateCalculator>>())
            .AddTransient<IRebateCalculatorFactory, RebateCalculatorFactory>()
            .BuildServiceProvider();

        //do the actual work here
        var rebateService= serviceProvider.GetService<IRebateService>();
        var request = InputRequest();

        rebateService.Calculate(request);
    }

    private static CalculateRebateRequest InputRequest()
    {
        Console.Clear();
        Console.WriteLine("Please provide a RebateIdentifier");
        var rebateId = Console.ReadLine();
        Console.WriteLine("Please provide a ProductIdentifier");
        var productId = Console.ReadLine();
        Console.WriteLine("Please provide a Volume");
        var volume = decimal.Parse(Console.ReadLine());
        return new CalculateRebateRequest()
        {
            ProductIdentifier = productId, RebateIdentifier = rebateId, Volume = volume = volume
        };
    }
}
