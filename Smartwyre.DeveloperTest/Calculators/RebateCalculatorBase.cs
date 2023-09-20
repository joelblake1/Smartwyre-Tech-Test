using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public abstract class RebateCalculatorBase
{
    private readonly IDataReader<Product> _productReader;
    private readonly IDataReader<Rebate> _rebateReader;
    private readonly IRebateDataWriter _rebateWriter;

    protected RebateCalculatorBase(IDataReader<Product> productReader, IDataReader<Rebate> rebateReader, IRebateDataWriter rebateWriter)
    {
        _productReader = productReader;
        _rebateReader = rebateReader;
        _rebateWriter = rebateWriter;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = GetRebate(request.RebateIdentifier);
        var product = GetProduct(request.ProductIdentifier);

        if (!rebate.IsValid() || 
            !product.IsValid(ProductIncentiveType) ||
            !CustomIsValid(rebate,request, product)
           )
        {
            return CalculateRebateResult.Failure();
        }

        var rebateAmount = GetRebateAmount(rebate,request,product);
        StoreRebate(rebate,rebateAmount);
        return CalculateRebateResult.Success(rebateAmount);
    }
    public abstract IncentiveType IncentiveType { get; }
    public abstract SupportedIncentiveType ProductIncentiveType { get; }
    protected abstract bool CustomIsValid(Rebate rebate, CalculateRebateRequest request, Product product);
    protected abstract decimal GetRebateAmount(Rebate rebate, CalculateRebateRequest request, Product product);
    private Product GetProduct(string identifier) 
        => _productReader.Get(identifier);
    private Rebate GetRebate(string identifier) 
        => _rebateReader.Get(identifier);
    private void StoreRebate(Rebate account, decimal rebateAmount) 
        => _rebateWriter.StoreCalculationResult(account,rebateAmount);
}