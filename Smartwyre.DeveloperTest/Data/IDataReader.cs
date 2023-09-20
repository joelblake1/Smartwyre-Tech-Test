namespace Smartwyre.DeveloperTest.Data;

public interface IDataReader<TResult> where TResult:class
{
    TResult Get(string identifier);
}