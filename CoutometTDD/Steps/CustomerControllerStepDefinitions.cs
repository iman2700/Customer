using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Domain.Entitiy;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace CoutometBDD.Steps
{
    [Binding]
    public class CustomerSteps
    {
        private HttpClient client;
        private HttpResponseMessage response;
        private Customer customer;
        private List<Customer> allCustomers;

        [Given(@"a customer request with valid data")]
        public void GivenACustomerRequestWithValidData()
        {
            customer = new Customer
            {
                Id=1,
                FirstName = "iman",
                LastName = "amiri",
                Email = "iman@example.com",
                PhoneNumber = "+989123231234",
                BankAccountNumber= "345323"
            };
        }

        public CustomerSteps()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7195/")
            };
        }

        [When(@"the request is sent to the CreateCustomer endpoint")]
        public void WhenTheRequestIsSentToTheCreateCustomerEndpoint()
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            response = client.PostAsync("CreateCustomer", content).Result;
        }

        [Then(@"the response should have a status code of 200 OK")]
        public void ThenTheResponseShouldHaveAStatusCodeOfOK()
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Then(@"the response body should contain the created customer details")]
        public void ThenTheResponseBodyShouldContainTheCreatedCustomerDetails()
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var createdCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

            Assert.AreEqual(customer.FirstName, createdCustomer.FirstName);
            Assert.AreEqual(customer.LastName, createdCustomer.LastName);
            Assert.AreEqual(customer.Email, createdCustomer.Email);
            Assert.AreEqual(customer.PhoneNumber, createdCustomer.PhoneNumber);
            Assert.AreEqual(customer.BankAccountNumber, createdCustomer.BankAccountNumber);
             
        }

         

        [When(@"the ID is provided in a request to the GetCustomerByUserId endpoint")]
        public void WhenTheIDIsProvidedInARequestToTheGetCustomerByUserIdEndpoint()
        {
            response = client.GetAsync($"GetCustomerByUserId/{customer.Id}").Result;
        }

        [Then(@"the response body should contain the customer details")]
        public void ThenTheResponseBodyShouldContainTheCustomerDetails()
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var retrievedCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

            Assert.AreEqual(customer.Id, retrievedCustomer.Id);
            Assert.AreEqual(customer.FirstName, retrievedCustomer.FirstName);
            Assert.AreEqual(customer.LastName, retrievedCustomer.LastName);
            Assert.AreEqual(customer.Email, retrievedCustomer.Email);
            Assert.AreEqual(customer.PhoneNumber, retrievedCustomer.PhoneNumber);
            Assert.AreEqual(customer.BankAccountNumber, retrievedCustomer.BankAccountNumber);
            
        }

        [When(@"a request is sent to the GetAllCustomer endpoint")]
        public void WhenARequestIsSentToTheGetAllCustomerEndpoint()
        {
            response = client.GetAsync("GetAllCustomer").Result;
        }

        [Then(@"the response body should contain a list of all customers")]
        public void ThenTheResponseBodyShouldContainAListOfAllCustomers()
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;
            allCustomers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

            Assert.IsTrue(allCustomers.Count > 0);
           
        }

        [Given(@"a customer update request with valid data")]
        public void GivenACustomerUpdateRequestWithValidData()
        {
            customer.FirstName = "iman";
            customer.LastName = "amiri";
            customer.PhoneNumber = "+989131231243";
            customer.Email = "imand@mail.com";
            customer.BankAccountNumber = "12345";
          
        }

        [When(@"the request is sent to the UpdateCustomer endpoint")]
        public void WhenTheRequestIsSentToTheUpdateCustomerEndpoint()
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            response = client.PutAsync($"UpdateCustomer/{customer.Id}", content).Result;
        }

        [Then(@"the response should have a status code of 204 No Content")]
        public void ThenTheResponseShouldHaveAStatusCodeOfNoContent()
        {
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [When(@"the ID is provided in a request to the DeleteCustomer endpoint")]
        public void WhenTheIDIsProvidedInARequestToTheDeleteCustomerEndpoint()
        {
            response = client.DeleteAsync($"DeleteCustomer/{customer.Id}").Result;
        }

        

        [AfterScenario]
        public void Cleanup()
        {
            client.Dispose();
        }
    }

    
}
