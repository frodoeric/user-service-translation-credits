Feature: Update users

  As an API client
  I want to be able to update user information
  So that I can keep user data current

  Scenario: Update a user successfully
    Given a UserUpdateRequest
    When I send a PUT request to /users/{id}
    Then I get an OK response
    And the user data is updated

  Scenario: Attempt to update a user with invalid data
    Given a UserUpdateRequest
    When I send a PUT request to /users/{id}
    Then I get a Bad Request response with an error message
