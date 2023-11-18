using API.Controllers;
using Application.Features.Commands.Customers.CreateCustomer;
using Application.Features.Commands.Customers.UpdateCustomer;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerApiTests.Controller;

 
    public class CustomerControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;

        public CustomerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task CreateCustomer_ShouldCreateCustomerAndReturnOkResponse()
        {
             
            var controller = new CustomerController(_mediatorMock.Object);
            var command = new CreateCustomerCommand()
            {
                Id = 1, FirstName = "Iman",LastName = "Amiri", Email = "iman@example.com"
                , PhoneNumber = "34534534534",BankAccountNumber = "345345354",DateOfBirth = DateTime.Now
            };

            
        var result = await controller.CreateCustomer(command);
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);

        Assert.NotNull(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetCustomerByUserId_ShouldGetCustomerAndReturnOkResponse()
        {
             
            var controller = new CustomerController(_mediatorMock.Object);
            var customerId = 1;

            
            var result = await controller.GetCustomerByUserId(customerId);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);


            Assert.NotNull(result);
            Assert.Equal(200, okObjectResult.StatusCode); 
 
    }

        [Fact]
        public async Task GetAllCustomer_ShouldGetAllCustomersAndReturnOkResponse()
        {
        var controller = new CustomerController(_mediatorMock.Object);
        var result = await controller.GetAllCustomer();
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        [HttpPut(Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task UpdateCustomer_ShouldUpdateCustomerAndReturnNoContentResponse()
        {
            // Arrange
            var controller = new CustomerController(_mediatorMock.Object);
            var command = new UpdateCustomerCommand()
            {
                Id = 1, FirstName = "Iman",LastName = "Amiri", Email = "iman@example.com"
                , PhoneNumber = "+989121233223",BankAccountNumber = "345345354",DateOfBirth = DateTime.Now
            };

        var result = await controller.UpdateCustomer(command);
        var objectResult = result as ObjectResult;

        Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldDeleteCustomerAndReturnNoContentResponse()
        {
            var controller = new CustomerController(_mediatorMock.Object);
            var customerId = 1;
            var result = await controller.DeleteCustomer(customerId);
        var objectResult = result as ObjectResult;
        Assert.Equal(200, objectResult.StatusCode);
        }
    }
 