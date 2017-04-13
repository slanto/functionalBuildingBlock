namespace ApplyingFunctionalProgramming.Tests
{
    using ApplyingFunctionalProgramming.Core;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class CustomerServiceTest
    {
        [Fact]
        public void RefillBalance_WhenCustomerIdNotExists_CustomerIsNotFoun()
        {
            var sut = new CustomerService(
                new Mock<ICustomerRepository>().Object,
                new Mock<ILogger>().Object,
                new Mock<IPaymentGateway>().Object);

            var result = sut.RefillBalance(0, 120);

            result.Should().NotBeEmpty().And.Be("Customer is not found");
        }
    }
}
