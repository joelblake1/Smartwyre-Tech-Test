using System;
using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    private readonly Func<IEnumerable<IRebateCalculator>> _getImplementations;
    private readonly IDataReader<Rebate> _rebateReader;

    public RebateCalculatorFactory(Func<IEnumerable<IRebateCalculator>> getImplementations,IDataReader<Rebate> rebateReader)
    {
        _getImplementations = getImplementations;
        _rebateReader = rebateReader;
    }

    public IRebateCalculator Create(CalculateRebateRequest request)
    {
        var rebate = _rebateReader.Get(request.RebateIdentifier);
        return _getImplementations().Single(x => x.IncentiveType == rebate.Incentive);
    }
}