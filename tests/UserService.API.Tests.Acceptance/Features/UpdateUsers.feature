Feature: Update users

  As an API client
  I want to be able to update user information
  So that I can keep user data current

  Scenario: Update a user successfully
    Given an existing user
    And a UserUpdateRequest
    When I send a PUT request to /users/{id}
    Then I get an OK response
    And the user is updated