using System.Net;
using Application.DTOs;
using Application.Exception;
using Application.Features.Commands.Customers.CreateCustomer;
using Application.Features.Commands.Customers.DeleteCustomer;
using Application.Features.Commands.Customers.UpdateCustomer;
using Application.Features.Queries.GetCustomer;
using Application.Features.Queries.GetCustomerList;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    // Constructor to inject an instance of IMediator through dependency injection
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateCustomer")]
    [ProducesResponseType(typeof(IEnumerable<CustomerCreateDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<int>>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        try
        {
            // Send the Create command to the handler
            var customerId = await _mediator.Send(command);

            // Return a success response if the update is successful
            return Ok(new { Message = $"Customer With Id {customerId} Create successfully" });

        }
        catch (ValidationException ex)
        {
            // Return a bad request response with the validation errors
            return BadRequest(new { Errors = ex.Errors });
        }
    }


    [HttpGet("{customerId}", Name = "GetCustomerByUserId")]
    [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Customer>> GetCustomerByUserId(int customerId)
    {
        try
        {
            // Send the query to the mediator for processing
            var customer = await _mediator.Send(new GetCustomerQuery(customerId));

            // Return a 200 OK response with the retrieved customer data
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Errors = ex.Message });
        }
    }


    [HttpGet(Name = "GetAllCustomer")]
    [ProducesResponseType(typeof(IReadOnlyList<Customer>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Customer>>> GetAllCustomer()
    {
        try
        {
            // Send a query to get a list of all customers to the mediator for processing
            var customer = await _mediator.Send(new GetCustomerListQuery());

            // Return a 200 OK response with the list of customers
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Errors = ex.Message });
        }
    }

    
    [HttpPut(Name = "UpdateCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerCommand command)
    {

        try
        {
            // Send the update command to the handler
            await _mediator.Send(command);

            // Return a success response if the update is successful
            return Ok(new { Message = "Customer updated successfully" });
        }
        catch (ValidationException ex)
        {
            
            // Return a bad request response with the validation errors
            return BadRequest(new { Errors = ex.Errors });
        }
        
    }

    [HttpDelete("{id}", Name = "DeleteCustomer")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        // Create a command to delete a customer by ID
        var command = new DeleteCustomerCommand() { Id = id };

        // Send the delete command to the mediator for processing
        await _mediator.Send(command);
        return Ok(new { Message = "Customer successfully Deleted" });
    }

}
