using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IDataReader<Product> 
{
    public Product Get(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return new Product();
    }
}
