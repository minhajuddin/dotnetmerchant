Feature: Authorize_Net Resource
  In order test the base controller functionality
  As a REST Client
  I want to be able to initiate credit card transactions and receive status messages

############################################## Authorize ##########################################################

  Scenario: Authorize endpoint of the Authorize_Net resource should validate a valid transaction and return xml
    Given the following parameters
      | name       | value         |
      | amount     | 1000          |
      | test       | true          |
    And the AuthorizeNet gateway's valid visa details
    And the AuthorizeNet credentials
    When I post to the Authorize endpoint of the AuthorizeNet resource as xml
    Then the response code should indicate success
    And the response should contain "<authorization>"
    And the response should contain "<approved>true</approved>"
    And the response should contain "<cvv_result>"
    And the response should contain "<avs_result>"

  Scenario: Authorize endpoint of the AuthorizeNet resource should not validate an invalid transaction and return xml
    Given the following parameters
      | name        | value         |
      | amount      | 200           |
      | test        | true          |
      | gatewaytest | true          |
    And the AuthorizeNet credentials
    And the AuthorizeNet gateway's invalid visa details
    When I post to the Authorize endpoint of the AuthorizeNet resource as xml
    Then the response code should indicate unauthorized
    And the response should contain "<authorization>"
    And the response should contain "<approved>false</approved>"

  Scenario: Authorize endpoint of the AuthorizeNet resource should validate a valid transaction and return json
    Given the following parameters
      | name       | value          |
      | amount     | 1823           |
      | test       | true           |
    And the AuthorizeNet credentials
    And the AuthorizeNet gateway's valid amex details
    When I post to the Authorize endpoint of the AuthorizeNet resource as json
    Then the response code should indicate success
    And the response should contain json property "approved" with value true
    And the response should contain json property "avs_result"
    And the response should contain json property "cvv_result"

  Scenario: Authorize endpoint of the AuthorizeNet resource should not validate an invalid transaction and return json
    Given the following parameters
      | name        | value         |
      | amount      | 200           |
      | test        | true          |
      | gatewaytest | true          |
    And the AuthorizeNet credentials
    And the AuthorizeNet's invalid visa details
    When I post to the Authorize endpoint of the AuthorizeNet resource as json
    Then the response code should indicate unauthorized
    And the response should contain json property "approved" with value false

  Scenario: Authorize endpoint of the AuthorizeNet resource should only allow POST requests
    When I get the Authorize endpoint of the AuthorizeNet resource as json
    Then the response code should indicate that the method is not allowed

############################################## Purchase ##########################################################

  Scenario: Purchase endpoint of the AuthorizeNet resource should validate a valid transaction and return xml
    Given the following parameters
      | name       | value           |
      | amount     | 1433            |
      | test       | true            |
    And the AuthorizeNet credentials
    And the AuthorizeNet gateway's valid amex details
    When I post to the Purchase endpoint of the AuthorizeNet resource as xml
    Then the response code should indicate success
    And the response should contain "<purchase>"
    And the response should contain "<approved>true</approved>"
    And the response should contain "<cvv_result>"
    And the response should contain "<avs_result>"

  Scenario: Purchase endpoint of the AuthorizeNet resource should not validate an invalid transaction and return xml
    Given the following parameters
      | name        | value         |
      | amount      | 200           |
      | test        | true          |
      | gatewaytest | true          |
    And the AuthorizeNet credentials
    And the AuthorizeNet gateway's invalid visa details
    When I post to the Purchase endpoint of the AuthorizeNet resource as xml
    Then the response code should indicate unauthorized
    And the response should contain "<purchase>"
    And the response should contain "<approved>false</approved>"

  Scenario: Purchase endpoint of the AuthorizeNet resource should validate a valid transaction and return json
    Given the following parameters
      | name       | value         |
      | amount     | 1859          |
      | test       | true          |
    And the AuthorizeNet credentials
    And the AuthorizeNet gateway's valid visa2 details
    When I post to the Purchase endpoint of the AuthorizeNet resource as json
    Then the response code should indicate success
    And the response should contain json property "approved" with value true

  Scenario: Purchase endpoint of the AuthorizeNet resource should not validate an invalid transaction and return json
    Given the following parameters
      | name        | value         |
      | amount      | 200           |
      | test        | true          |
      | gatewaytest | true          |
    And the AuthorizeNet gateway's invalid visa details
    And the AuthorizeNet credentials
    When I post to the Purchase endpoint of the AuthorizeNet resource as json
    Then the response code should indicate unauthorized
    And the response should contain json property "approved" with value false

  Scenario: Purchase endpoint of the AuthorizeNet resource should only allow POST requests
    When I get the Purchase endpoint of the AuthorizeNet resource as json
    Then the response code should indicate that the method is not allowed

#Haven't figured out how to get authorize.net's void method to work in test mode since all test transactions
# have a tranaction id of 0
############################################## Void ##########################################################

#  Scenario: Void endpoint of the AuthorizeNet resource should cancel a pre-authorized transaction, returning json
#    Given the following parameters
#      | name   | value |
#      | number | 0     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Void endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate success
#    And the response should contain json property "approved" with value true
#
#  Scenario: Void endpoint of the AuthorizeNet resource should cancel a pre-authorized transaction, returning xml
#    Given the following parameters
#      | name   | value |
#      | number | 0     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Void endpoint of the AuthorizeNet resource as xml
#    Then the response code should indicate success
#    And the response should contain "<void>"
#    And the response should contain "<approved>true</approved>"
#
#  Scenario: Void endpoint of the AuthorizeNet resource should not cancel a phony transaction, returning json
#    Given the following parameters
#      | name   | value |
#      | amount | 1000  |
#      | ident  | 2     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Void endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate unauthorized
#    And the response should contain json property "approved" with value false
#
#  Scenario: Void endpoint of the AuthorizeNet resource should not cancel a phony transaction, returning xml
#    Given the following parameters
#      | name   | value |
#      | amount | 1000  |
#      | ident  | 2     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Void endpoint of the AuthorizeNet resource as xml
#    Then the response code should indicate unauthorized
#    And the response should contain "<void>"
#    And the response should contain "<approved>false</approved>"
#
#  Scenario: Void endpoint of the AuthorizeNet resource should only allow POST requests
#    Given the following parameters
#      | name | value |
#      | fake | param |
#    And the AuthorizeNet credentials
#    When I get the Void endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate that the method is not allowed


#TODO: alter capture tests to be a two-step auth and capture test so we can pass a valid transid to the capture endpoint
############################################## Capture ##########################################################
#
#  Scenario: Capture endpoint of the AuthorizeNet resource should complete a pre-authorized transaction, returning json
#    Given the following parameters
#      | name   | value |
#      | number | 0     |
#      | amount | 10000 |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Capture endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate success
#    And the response should contain json property "approved" with value true
#
#  Scenario: Cature endpoint of the AuthorizeNet resource should complete a pre-authorized transaction, returning xml
#    Given the following parameters
#      | name   | value |
#      | number | 0     |
#      | amount | 10000 |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Capture endpoint of the AuthorizeNet resource as xml
#    Then the response code should indicate success
#    And the response should contain "<capture>"
#    And the response should contain "<approved>true</approved>"
#
#  Scenario: Capture endpoint of the AuthorizeNet resource should not complete a phony transaction, returning xml
#    Given the following parameters
#      | name   | value |
#      | amount | 1000  |
#      | ident  | 2     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Capture endpoint of the AuthorizeNet resource as xml
#    Then the response code should indicate unauthorized
#    And the response should contain "<capture>"
#    And the response should contain "<approved>false</approved>"
#
#  Scenario: Capture endpoint of the AuthorizeNet resource should not complete a phony transaction, returning json
#    Given the following parameters
#      | name   | value |
#      | amount | 1000  |
#      | ident  | 2     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Capture endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate unauthorized
#    And the response should contain json property "approved" with value false
#
#  Scenario: Capture endpoint of the AuthorizeNet resource should only allow POST requests
#    When I get the Capture endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate that the method is not allowed


# not sure if it's possible to do test credit transactions on authorize.net

############################################## Credit ##########################################################
#
#  Scenario: Credit endpoint of the AuthorizeNet resource should complete a pre-authorized transaction, returning json
#    Given the following parameters
#      | name   | value |
#      | number | 1     |
#      | amount | 10000 |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Credit endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate success
#    And the response should contain json property "approved" with value true
#
#  Scenario: Cature endpoint of the AuthorizeNet resource should complete a pre-authorized transaction, returning xml
#    Given the following parameters
#      | name   | value |
#      | number | 1     |
#      | amount | 10000 |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Credit endpoint of the AuthorizeNet resource as xml
#    Then the response code should indicate success
#    And the response should contain "<credit>"
#    And the response should contain "<approved>true</approved>"
#
#  Scenario: Credit endpoint of the AuthorizeNet resource should not complete a phony transaction, returning xml
#    Given the following parameters
#      | name   | value |
#      | amount | 1000  |
#      | ident  | 2     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Credit endpoint of the AuthorizeNet resource as xml
#    Then the response code should indicate unauthorized
#    And the response should contain "<credit>"
#    And the response should contain "<approved>false</approved>"
#
#  Scenario: Credit endpoint of the AuthorizeNet resource should not complete a phony transaction, returning json
#    Given the following parameters
#      | name   | value |
#      | amount | 1000  |
#      | ident  | 2     |
#      | test   | true  |
#    And the AuthorizeNet credentials
#    When I post to the Credit endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate unauthorized
#    And the response should contain json property "approved" with value false
#
#  Scenario: Credit endpoint of the AuthorizeNet resource should only allow POST requests
#    Given the following parameters
#      | name | value |
#      | fake | param |
#    And the AuthorizeNet credentials
#    When I get the Credit endpoint of the AuthorizeNet resource as json
#    Then the response code should indicate that the method is not allowed
