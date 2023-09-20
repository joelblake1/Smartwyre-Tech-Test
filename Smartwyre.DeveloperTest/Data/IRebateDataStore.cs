using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public interface IRebateDataWriter{
    void StoreCalculationResult(Rebate account, decimal rebateAmount);
}