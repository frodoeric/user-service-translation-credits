Feature: translation credits

  As an API client
  I want to be able to manage credits for an user
  So that I can manage their credit balance as needed

  Scenario: Successfully add credits to a user's account
    Given an existing user
    And a TranslationCreditsRequest with 10 credits
    When I send a POST request to /users/{id}/credits/add
    Then I get an OK response
    And the user's credits will be equal to 10

  Scenario: Successfully spend credits from a user's account
    Given an existing user with 10 credits
    And a TranslationCreditsRequest with 5 credits
    When I send a POST request to /users/{id}/credits/spend
    Then I get an OK response
    And the user's credits will be equal to 5