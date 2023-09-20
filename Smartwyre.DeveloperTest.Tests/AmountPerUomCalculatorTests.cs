using System.Collections.Generic;
using Moq;
using Shouldly;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class AmountPerUomCalculatorTests
{
    private readonly Mock<IDataReader<Rebate>> _mockRebateReader;
    private readonly Mock<IDataReader<Product>> _mockProductReader;
    private readonly Mock<IRebateDataWriter> _mockRebateWriter;
    private readonly AmountPerUomCalculator _sut;
    private static Product ValidProduct =>new()
    {
        SupportedIncentives = SupportedIncentiveType.AmountPerUom
    };
    private static Rebate ValidRebate => new ()
    {
        Amount = 10
    };
    private static Rebate InvalidRebate => new ()
    {
        Amount = 0
    };
    private static Product InvalidProduct => new ()
    {
        SupportedIncentives = SupportedIncentiveType.FixedCashAmount
    };
    public static IEnumerable<object[]> InvalidProducts => new []
    {
        new object[]{InvalidProduct},
        new object[]{null}
    };
    public static IEnumerable<object[]> InvalidRebates => new []
    {
        new object[]{InvalidRebate},
        new object[]{null}
    };
    
    private static CalculateRebateRequest Request = new()
    {
        RebateIdentifier = "my-rebate",
        ProductIdentifier= "my-product",
        Volume = 10
    };
    private static CalculateRebateRequest InvalidRequest = new()
    {
        RebateIdentifier = "my-rebate",
        ProductIdentifier= "my-product"
    };
    public AmountPerUomCalculatorTests()
    {
        _mockRebateReader = new Mock<IDataReader<Rebate>>();
        _mockProductReader = new Mock<IDataReader<Product>>();
        _mockRebateWriter= new Mock<IRebateDataWriter>();
        _sut = new AmountPerUomCalculator(_mockProductReader.Object,_mockRebateReader.Object, _mockRebateWriter.Object);
        
    }
    
    [Fact]
    public void Calculate_Success()
    {
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == Request.RebateIdentifier)))
            .Returns(ValidRebate);

        _mockProductReader.Setup(x => x.Get(It.Is<string>(x => x == Request.ProductIdentifier)))
            .Returns(ValidProduct);

        _mockRebateWriter.Setup(x => x.StoreCalculationResult(ValidRebate, It.IsAny<decimal>()));

        var result = _sut.Calculate(Request);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBe(true);
        result.Amount.ShouldBe(100);
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(It.Is<Rebate>(y => y.Amount==10), 100),Times.Once);
        _mockRebateReader.Verify(x=>x.Get(Request.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(Request.ProductIdentifier),Times.Once);
    }
    [Theory]
    [MemberData(memberName:nameof(InvalidProducts))]
    public void Calculate_InvalidProduct_Fail(Product product)
    {
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == Request.RebateIdentifier)))
            .Returns(ValidRebate);

        _mockProductReader.Setup(x => x.Get(It.Is<string>(x => x == Request.ProductIdentifier)))
            .Returns(product);

        _mockRebateWriter.Setup(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()));

        var result = _sut.Calculate(Request);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBe(false);
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(ValidRebate, 1000), Times.Never);
        _mockRebateReader.Verify(x=>x.Get(Request.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(Request.ProductIdentifier),Times.Once);
    }
    [Theory]
    [MemberData(memberName:nameof(InvalidRebates))]
    public void Calculate_InvalidRebate_Fail(Rebate rebate)
    {
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == Request.RebateIdentifier)))
            .Returns(rebate);

        _mockProductReader.Setup(x => x.Get(It.Is<string>(x => x == Request.ProductIdentifier)))
            .Returns(ValidProduct);

        _mockRebateWriter.Setup(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()));

        var result = _sut.Calculate(Request);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBe(false);
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(rebate, 100), Times.Never);
        _mockRebateReader.Verify(x=>x.Get(Request.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(Request.ProductIdentifier),Times.Once);
    }
    [Fact]
    public void Calculate_InvalidRequest_Fail()
    {
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == InvalidRequest.RebateIdentifier)))
            .Returns(ValidRebate);

        _mockProductReader.Setup(x => x.Get(It.Is<string>(x => x == InvalidRequest.ProductIdentifier)))
            .Returns(ValidProduct);

        _mockRebateWriter.Setup(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()));

        var result = _sut.Calculate(InvalidRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBe(false);
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(ValidRebate, 100), Times.Never);
        _mockRebateReader.Verify(x=>x.Get(InvalidRequest.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(InvalidRequest.ProductIdentifier),Times.Once);
    }
}