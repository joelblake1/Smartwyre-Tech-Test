using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Extensions;

public static class ProductExtensions
{
    public static bool IsValid(this Product product, SupportedIncentiveType incentiveType) 
        => product!= null && 
           product.SupportedIncentives.HasFlag(incentiveType);
}