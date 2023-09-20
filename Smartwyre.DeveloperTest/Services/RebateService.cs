using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateCalculatorFactory _calculatorFactory;

    public RebateService(IRebateCalculatorFactory calculatorFactory)
    {
        _calculatorFactory = calculatorFactory;
    }
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var calculator = _calculatorFactory.Create(request);
        return calculator.Calculate(request);
    }

}