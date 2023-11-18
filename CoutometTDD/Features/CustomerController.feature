Feature: Customer Controller

    Scenario Outline: CRUD Operations for Customer
        Given a customer request with valid data
        When the request is sent to the CreateCustomer endpoint
        Then the response should have a status code of 200 OK
        And the response body should contain the created customer details

        Given a customer ID
        When the ID is provided in a request to the GetCustomerByUserId endpoint
        Then the response should have a status code of 200 OK
        And the response body should contain the customer details

        When a request is sent to the GetAllCustomer endpoint
        Then the response should have a status code of 200 OK
        And the response body should contain a list of all customers

        Given a customer update request with valid data
        When the request is sent to the UpdateCustomer endpoint
        Then the response should have a status code of 204 No Content

        Given a customer ID
        When the ID is provided in a request to the DeleteCustomer endpoint
        Then the response should have a status code of 204 No Content