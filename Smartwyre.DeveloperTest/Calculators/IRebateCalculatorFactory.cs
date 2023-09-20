using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public interface IRebateCalculatorFactory
{
    IRebateCalculator Create(CalculateRebateRequest request);
}