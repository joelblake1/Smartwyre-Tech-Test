using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class AmountPerUomCalculator : RebateCalculatorBase,IRebateCalculator
{
    public AmountPerUomCalculator(IDataReader<Product> productReader, IDataReader<Rebate> rebateReader, IRebateDataWriter rebateWriter) :
        base(productReader, rebateReader, rebateWriter)
    {
    }
        
    public override SupportedIncentiveType ProductIncentiveType => SupportedIncentiveType.AmountPerUom;
    public override IncentiveType IncentiveType => IncentiveType.AmountPerUom;
    protected override bool CustomIsValid(Rebate rebate, CalculateRebateRequest request = null, Product product = null)
    {
        return rebate.Amount != 0 && 
               request.Volume != 0;
    }
    protected override decimal GetRebateAmount(Rebate rebate, CalculateRebateRequest request, Product product)
    {
        return rebate.Amount * request.Volume;
    }
}