using System;
using System.Collections.Generic;
using Moq;
using Shouldly;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedCashAmountRebateCalculatorTests
{
    private readonly Mock<IDataReader<Rebate>> _mockRebateReader;
    private readonly Mock<IDataReader<Product>> _mockProductReader;
    private readonly Mock<IRebateDataWriter> _mockRebateWriter;
    private readonly FixedCashAmountRebateCalculator _sut;
    private static Product ValidProduct =>new()
    {
        SupportedIncentives = SupportedIncentiveType.FixedCashAmount
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
        SupportedIncentives = SupportedIncentiveType.AmountPerUom
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
        ProductIdentifier= "my-product"
    };
    public FixedCashAmountRebateCalculatorTests()
    {
        _mockRebateReader = new Mock<IDataReader<Rebate>>();
        _mockProductReader = new Mock<IDataReader<Product>>();
        _mockRebateWriter= new Mock<IRebateDataWriter>();
        _sut = new FixedCashAmountRebateCalculator(_mockProductReader.Object,_mockRebateReader.Object, _mockRebateWriter.Object);
        
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
        result.Amount.ShouldBe(10);
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(It.Is<Rebate>(y => y.Amount==10), 10),Times.Once);
        _mockRebateReader.Verify(x=>x.Get(Request.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(Request.ProductIdentifier),Times.Once);
    }
    [Theory]
    [MemberData(memberName:nameof(InvalidProducts))]
    public void Calculate_InvalidProduct_Fail(Product product)
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "my-rebate",
            ProductIdentifier= "my-product"
        };
        
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == request.RebateIdentifier)))
            .Returns(ValidRebate);

        _mockProductReader.Setup(x => x.Get(It.Is<string>(x => x == request.ProductIdentifier)))
            .Returns(product);

        _mockRebateWriter.Setup(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()));

        var result = _sut.Calculate(request);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBe(false);
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(ValidRebate, 10), Times.Never);
        _mockRebateReader.Verify(x=>x.Get(request.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(request.ProductIdentifier),Times.Once);
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
        
        _mockRebateWriter.Verify(x=>x.StoreCalculationResult(rebate, 10), Times.Never);
        _mockRebateReader.Verify(x=>x.Get(Request.RebateIdentifier),Times.Once);
        _mockProductReader.Verify(x=>x.Get(Request.ProductIdentifier),Times.Once);
    }
}