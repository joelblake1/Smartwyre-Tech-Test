using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedRateRebateCalculator : RebateCalculatorBase,IRebateCalculator
{
    public FixedRateRebateCalculator(IDataReader<Product> productReader, IDataReader<Rebate> rebateReader, IRebateDataWriter rebateWriter) :
        base(productReader, rebateReader, rebateWriter)
    {
    }

    public override SupportedIncentiveType ProductIncentiveType => SupportedIncentiveType.FixedRateRebate;
    public override IncentiveType IncentiveType => IncentiveType.FixedRateRebate;
    protected override bool CustomIsValid(Rebate rebate, CalculateRebateRequest request = null, Product product = null)
    {
        return rebate.Percentage != 0 && 
               product.Price != 0 && 
               request.Volume != 0;
    }
    protected override decimal GetRebateAmount(Rebate rebate, CalculateRebateRequest request, Product product)
    {
        return product.Price * rebate.Percentage * request.Volume;
    }
}