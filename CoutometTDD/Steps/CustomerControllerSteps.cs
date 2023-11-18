 

namespace CoutometTDD.Steps;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

[Binding]
public class CustomerControllerSteps
{

    private readonly HttpClient _httpClient;

    public CustomerControllerSteps()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000");
    }

    private HttpResponseMessage response;
    private Customer createdCustomer;
    private List<Customer> allCustomers;

    [Given(@"a customer request with valid data")]
    public void GivenACustomerRequestWithValidData()
    {
        // Prepare valid customer data for the request
        var customerData = new Customer
        {
            Name = "John Doe",
            Email = "johndoe@example.com",
            // Add other required fields as necessary
        };



        // Serialize customer data to JSON
        var json = JsonConvert.SerializeObject(customerData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send POST request to CreateCustomer endpoint
        response = _httpClient.PostAsync("api/customer", content).Result;
    }

    [When(@"the request is sent to the CreateCustomer endpoint")]
    public void WhenTheRequestIsSentToTheCreateCustomerEndpoint()
    {
        // No action needed here, as the request was already sent in the Given step
    }

    [Then(@"the response should have a status code of 200 OK")]
    public void ThenTheResponseShouldHaveAStatusCodeOf200OK()
    {
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Then(@"the response body should contain the created customer details")]
    public void ThenTheResponseBodyShouldContainTheCreatedCustomerDetails()
    {
        var responseBody = response.Content.ReadAsStringAsync().Result;
        createdCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

        // Assert that the created customer has valid data
        Assert.IsNotNull(createdCustomer);
        Assert.IsNotNull(createdCustomer.Id);
        Assert.AreEqual("John Doe", createdCustomer.Name);
        Assert.AreEqual("johndoe@example.com", createdCustomer.Email);
        // Assert other customer details as necessary
    }

    [Given(@"a customer ID")]
    public void GivenACustomerID()
    {
        // Assume that the ID of the created customer from the previous scenario is used here
        // You can store the created customer ID in a variable or retrieve it from the response body
        var customerId = createdCustomer.Id;


        // Send GET request to GetCustomerByUserId endpoint with the provided customer ID
        response = _httpClient.GetAsync($"api/customer/{customerId}").Result;
    }

    [When(@"the ID is provided in a request to the GetCustomerByUserId endpoint")]
    public void WhenTheIDIsProvidedInARequestToTheGetCustomerByUserIdEndpoint()
    {
        // No action needed here, as the request was already sent in the Given step
    }



    [Then(@"the response body should contain the customer details")]
    public void ThenTheResponseBodyShouldContainTheCustomerDetails()
    {
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var customer = JsonConvert.DeserializeObject<Customer>(responseBody);

        // Assert that the returned customer has valid data
        Assert.IsNotNull(customer);
        Assert.AreEqual(createdCustomer.Id, customer.Id);
        Assert.AreEqual(createdCustomer.Name, customer.Name);
        Assert.AreEqual(createdCustomer.Email, customer.Email);
        // Assert other customer details as necessary
    }

    [When(@"a request is sent to the GetAllCustomer endpoint")]
    public void WhenARequestIsSentToTheGetAllCustomerEndpoint()
    {

        // Send GET request to GetAllCustomer endpoint
        response = _httpClient.GetAsync("api/customers").Result;
    }

    [Then(@"the response body should contain a list of all customers")]
    public void ThenTheResponseBodyShouldContainAListOfAllCustomers()
    {
        var responseBody = response.Content.ReadAsStringAsync().Result;
        allCustomers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

        // Assert that the returned list of customers is not null or empty
        Assert.IsNotNull(allCustomers);
        Assert.IsTrue(allCustomers.Count > 0);
    }

    [Given(@"a customer update request with valid data")]
    public void GivenACustomerUpdateRequestWithValidData()
    {
        // Assume that an existing customer ID and updated customer data are provided here
        var customerId = createdCustomer.Id;
        var updatedCustomerData = new Customer
        {
            Id = customerId,
            Name = "Updated Name",
            Email = "updatedemail@example.com",
            // Add other fields to be updated as necessary
        };

 
        // Serialize updated customer data to JSON
        var json = JsonConvert.SerializeObject(updatedCustomerData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send PUT request to UpdateCustomer endpoint with the provided customer ID
        response = _httpClient.PutAsync($"api/customer/{customerId}", content).Result;
    }

    [When(@"the request is sent to the UpdateCustomer endpoint")]
    public void WhenTheRequestIsSentToTheUpdateCustomerEndpoint()
    {
        // No action needed here, as the request was already sent in the Given step
    }

    [Then(@"the response should have a status code of 204 No Content")]
    public void ThenTheResponseShouldHaveAStatusCodeOf204NoContent()
    {
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [When(@"the ID is provided in a request to the DeleteCustomer endpoint")]
    public void WhenTheIDIsProvidedInARequestToTheDeleteCustomerEndpoint()
    {
        // No action needed here, as the request was already sent in the Given step
    }

}
   