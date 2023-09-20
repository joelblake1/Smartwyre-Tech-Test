using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public interface IRebateCalculator
{
    CalculateRebateResult Calculate(CalculateRebateRequest request);
    IncentiveType IncentiveType { get; }
}