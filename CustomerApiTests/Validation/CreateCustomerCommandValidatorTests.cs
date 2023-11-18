using Application.Features.Commands.Customers.CreateCustomer;
using Application.Persistence;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace CustomerApiTests.Validation
{
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class CreateCustomerCommandValidatorTests
    {
        private readonly CreateCustomerCommandValidator _validator;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateCustomerCommandValidatorTests()
        {
            // Create mock instances for dependencies
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();

            // Instantiate the validator with the mock dependencies
            _validator = new CreateCustomerCommandValidator(_customerRepositoryMock.Object, _mapperMock.Object);



        }

        [Fact]
        public async Task Validate_Should_Fail_For_Empty_FirstName()
        {
            var command = new CreateCustomerCommand { FirstName = string.Empty, LastName = "amiri", Email = "johndoe@email.com", PhoneNumber = "+989131233243" };

            var result = await _validator.ValidateAsync(command);

            result.Errors.Should().Contain(error => error.PropertyName == "FirstName" && error.ErrorMessage == "First name is required.");
        }

        [Fact]
        public async Task Validate_Should_Fail_For_Empty_LastName()
        {
            var command = new CreateCustomerCommand { FirstName = "iman", LastName = string.Empty, Email = "iman@email.com", PhoneNumber = "+989131233243" };

            var result = await _validator.ValidateAsync(command);

            result.Errors.Should().Contain(error => error.PropertyName == "LastName" && error.ErrorMessage == "Last name is required.");
        }

        [Fact]
        public async Task Validate_Should_Fail_For_Invalid_Email()
        {
            var command = new CreateCustomerCommand { FirstName = "iman", LastName = "amiri", Email = "invalidEmail", PhoneNumber = "+989131233243" };

            var result = await _validator.ValidateAsync(command);

            result.Errors.Should().Contain(error => error.PropertyName == "Email" && error.ErrorMessage == "Invalid email address.");
        }


        [Fact]
        public async Task Validate_Should_Fail_For_PhoneNumber()
        {
            var command = new CreateCustomerCommand { FirstName = "iman", LastName = "amiri", Email = "iman@email.com", PhoneNumber = "+98789" };

            var result = await _validator.ValidateAsync(command);

            result.Errors.Should().Contain(error => error.PropertyName == "PhoneNumber" && error.ErrorMessage == "Phone Number is not valid");
        }

        [Fact]
        public async Task Validate_Should_Succeed_For_Valid_Command()
        {
            var command = new CreateCustomerCommand { FirstName = "iman", LastName = "amiri", Email = "iman@email.com", PhoneNumber = "+989131233243" };

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
