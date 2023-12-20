Feature: Get users

As an api client
I want to be able to query users
So that I can get their information

Scenario: Get all users
	Given two existing users
	When I send a GET request to /users
	Then I get the users

Scenario: Get a user by ID
	Given an existing user
	When I send a GET request to /users/{id}
	Then I get the user
