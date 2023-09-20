using System;
using Moq;
using Shouldly;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly RebateService _sut;
    private readonly Mock<IRebateCalculatorFactory> _mockCalculatorFactory;
    private readonly Mock<IRebateCalculator> _mockCalculator;

    public RebateServiceTests()
    {
        _mockCalculator = new Mock<IRebateCalculator>();
        _mockCalculatorFactory = new Mock<IRebateCalculatorFactory>();
        _sut = new RebateService(_mockCalculatorFactory.Object);
    }

    [Fact]
    public void Calculate_Success()
    {
        _mockCalculatorFactory.Setup(x => x.Create(It.IsAny<CalculateRebateRequest>()))
            .Returns(_mockCalculator.Object);
        _mockCalculator.Setup(x => x.Calculate(It.IsAny<CalculateRebateRequest>()))
            .Returns(new CalculateRebateResult());
        var request = new CalculateRebateRequest();

        var result = _sut.Calculate(request);
        result.ShouldNotBeNull();

        _mockCalculatorFactory.Verify(x=>x.Create(request),Times.Once);
        _mockCalculator.Verify(x=>x.Calculate(request),Times.Once);
    }
}