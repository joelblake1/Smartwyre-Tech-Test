namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public bool IsSuccess { get; set; }
    public decimal Amount { get; set; }

    public static CalculateRebateResult Failure()
    {
        return new() { IsSuccess = false};
    }
    public static CalculateRebateResult Success(decimal amount)
    {
        return new() { IsSuccess = true, Amount = amount};
    }

}

