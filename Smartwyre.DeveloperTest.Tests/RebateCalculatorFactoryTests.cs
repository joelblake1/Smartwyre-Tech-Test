using System;
using System.Collections.Generic;
using Moq;
using Shouldly;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateCalculatorFactoryTests
{
    private readonly RebateCalculatorFactory _sut;
    private readonly Mock<IDataReader<Rebate>> _mockRebateReader;
    private readonly Mock<IRebateCalculator> _mockCalculator1;
    private readonly Mock<IRebateCalculator> _mockCalculator2;
    private readonly Mock<IRebateCalculator> _mockCalculator3;

    public RebateCalculatorFactoryTests()
    {
        _mockRebateReader = new Mock<IDataReader<Rebate>>();
        _mockCalculator1 = new Mock<IRebateCalculator>();
        _mockCalculator2 = new Mock<IRebateCalculator>();
        _mockCalculator3 = new Mock<IRebateCalculator>();
        Func<IEnumerable<IRebateCalculator>> getCalculatorsFunction = () => new[]
        {
            _mockCalculator1.Object,
            _mockCalculator2.Object,
            _mockCalculator3.Object
        };
        _sut = new RebateCalculatorFactory(getCalculatorsFunction, _mockRebateReader.Object);
    }
    
    [Fact]
    public void Create_Success()
    {
        var request = new CalculateRebateRequest(){RebateIdentifier = "my-rebate"};
        _mockCalculator1.SetupGet(x => x.IncentiveType).Returns(IncentiveType.FixedCashAmount);
        _mockCalculator2.SetupGet(x => x.IncentiveType).Returns(IncentiveType.AmountPerUom);
        _mockCalculator3.SetupGet(x => x.IncentiveType).Returns(IncentiveType.FixedRateRebate);
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == request.RebateIdentifier)))
            .Returns(new Rebate(){Incentive = IncentiveType.AmountPerUom});


        var result = _sut.Create(request);
        result.ShouldNotBeNull();
        result.ShouldBe(_mockCalculator2.Object);

        _mockRebateReader.Verify(x=>x.Get(request.RebateIdentifier),Times.Once);
    }
    [Fact]
    public void Create_NoMatchingService_Fail()
    {
        var request = new CalculateRebateRequest(){RebateIdentifier = "my-rebate"};
        _mockCalculator1.SetupGet(x => x.IncentiveType).Returns(IncentiveType.FixedCashAmount);
        _mockCalculator3.SetupGet(x => x.IncentiveType).Returns(IncentiveType.FixedRateRebate);
        _mockRebateReader.Setup(x => x.Get(It.Is<string>(x => x == request.RebateIdentifier)))
            .Returns(new Rebate(){Incentive = IncentiveType.AmountPerUom});
        
        var result = Should.Throw<Exception>(() => _sut.Create(request));
        
        _mockRebateReader.Verify(x=>x.Get(request.RebateIdentifier),Times.Once);
    }
}