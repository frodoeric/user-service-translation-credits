Feature: translation credits

  As an API client
  I want to be able to manage credits for an user
  So that I can manage their credit balance as needed

  Scenario: Successfully add credits to a user's account
    Given an existing user
    And a TranslationCreditsRequest with 10 credits
    When I send a POST request to /users/{id}/credits/add
    Then I get an OK response
    And the user's credits are increased by 10