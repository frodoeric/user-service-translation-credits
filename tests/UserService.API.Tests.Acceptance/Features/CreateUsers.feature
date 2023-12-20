Feature: Create users

As an api client
I want to be able to create users
So that I can save their information

Scenario: Create a user
	Given a UserCreationRequest
	When I send a POST request to /users
	Then I get an OK response
	And the user is persisted

Scenario: Try to create a user with existing email
	Given an existing user
	And a UserCreationRequest with the same email
	When I send a POST request to /users
	Then I get a Bad Request response with an error message
