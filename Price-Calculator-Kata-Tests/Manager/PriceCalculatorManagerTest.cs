using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Price_Calculator_Kata;
using Price_Calculator_Kata.Models;

namespace Price_Calculator_Kata_Tests
{
    [TestClass]
    public class PriceCalculatorManagerTest
    {
        private Mock<IProductDetailsManager> _mockProductDetailsManager;
        private Mock<IPriceCalculatorStringBuilder> _mockPriceCalculatorStringBuilder;
        private Mock<IConfigurationWrapper> _mockConfig;
        private Mock<IConsoleWrapper> _mockConsoleWrapper;
        private PriceCalulatorManager _priceCalulatorManager;


        [TestInitialize]
        public void Setup()
        {
            _mockProductDetailsManager = new Mock<IProductDetailsManager>();
            _mockPriceCalculatorStringBuilder = new Mock<IPriceCalculatorStringBuilder>();
            _mockConfig = new Mock<IConfigurationWrapper>();
            _mockConsoleWrapper = new Mock<IConsoleWrapper>();
            _priceCalulatorManager = new PriceCalulatorManager(_mockProductDetailsManager.Object,
                _mockPriceCalculatorStringBuilder.Object, _mockConfig.Object, _mockConsoleWrapper.Object);
        }

        [TestMethod]
        public void GetAdditionalExpenses_WhenNoIsSelected_ThenZeroIsReturned()
        {
            var testProduct = new Product();

            var result = _priceCalulatorManager.GetAdditionalExpenses("N", testProduct);

            result.Should().Be(0M);
        }

        [TestMethod]
        public void GetAdditionalExpenses_WhenYesIsSelected_ThenTheCorrectPriceIsReturned()
        {
            var testProduct = new Product { Price = 20M };
            _mockPriceCalculatorStringBuilder.Setup(x => x.ApplyPercentagePrompt()).Returns("Apply percentage");
            _mockConfig.Setup(x => x.AdditionalExpensesPercentage).Returns(20);
            _mockConsoleWrapper.Setup(x => x.GetAnswer(It.IsAny<string>())).Returns("Y");

            var result = _priceCalulatorManager.GetAdditionalExpenses("Y", testProduct);

            result.Should().Be(4M);
        }
    }
}
