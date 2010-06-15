Feature: Bogus Resource
	In order test the base controller functionality
	As a REST Client
	I want to be able to initiate credit card transactions and receive status messages

Scenario: Authorize endpoint of the Bogus resource should validate a valid transaction and return xml
	Given the following parameters
		| name         | value            |
		| number       | 1                |
        | amount       | 1000             |
        | test         | true             |
		When I post to the Authorize endpoint of the Bogus resource as xml
		Then the response code should indicate success 
		And the response should contain "<authorization>"
        And the response should contain "<approved>true</approved>"

Scenario: Authorize endpoint of the Bogus resource should not validate an invalid transaction and return xml
	Given the following parameters
		| name         | value            |
		| number       | 2                |
        | amount       | 1000             |
        | test         | true             |
		When I post to the Authorize endpoint of the Bogus resource as xml
		Then the response code should indicate unauthorized
		And the response should contain "<authorization>"
        And the response should contain "<approved>false</approved>"		

Scenario: Authorize endpoint of the Bogus resource should validate a valid transaction and return json
      Given the following parameters
          | name         | value            |
          | number       | 1                |
          | amount       | 1000             |
          | test         | true             |
          When I post to the Authorize endpoint of the Bogus resource as json
          Then the response code should indicate success
          And the response should contain json property "approved" with value true

Scenario: Authorize endpoint of the Bogus resource should not validate an invalid transaction and return json
      Given the following parameters
          | name         | value            |
          | number       | 2                |
          | amount       | 1000             |
          | test         | true             |
          When I post to the Authorize endpoint of the Bogus resource as json
          Then the response code should indicate unauthorized
          And the response should contain json property "approved" with value false

Scenario: Void endpoint of the Bogus resource should cancel a pre-authorized transaction, returning json
      Given the following parameters
          | name         | value            |
          | number       | 1                |
          | test         | true             |
          When I post to the Void endpoint of the Bogus resource as json
          Then the response code should indicate success
          And the response should contain json property "approved" with value true

Scenario: Void endpoint of the Bogus resource should cancel a pre-authorized transaction, returning xml
      Given the following parameters
          | name         | value            |
          | number       | 1                |
          | test         | true             |
          When I post to the Void endpoint of the Bogus resource as xml
          Then the response code should indicate success
          And the response should contain "<void>"
          And the response should contain "<approved>true</approved>"

Scenario: Capture endpoint of the Bogus resource should not complete a phony transaction, returning xml
      Given the following parameters
          | name         | value            |
          | amount       | 1000             |
          | ident        | 2                |
          | test         | true             |
          When I post to the Capture endpoint of the Bogus resource as xml
          Then the response code should indicate unauthorized
          And the response should contain "<capture>"
          And the response should contain "<approved>false</approved>"

Scenario: Capture endpoint of the Bogus resource should not complete a phony transaction, returning json
      Given the following parameters
          | name         | value            |
          | amount       | 1000             |
          | ident        | 2                |
          | test         | true             |
          When I post to the Capture endpoint of the Bogus resource as json
          Then the response code should indicate unauthorized
          And the response should contain json property "approved" with value false

Scenario: Void endpoint of the Bogus resource should not cancel a phony transaction, returning json
      Given the following parameters
          | name         | value            |
          | amount       | 1000             |
          | ident        | 2                |
          | test         | true             |
          When I post to the Void endpoint of the Bogus resource as json
          Then the response code should indicate unauthorized
          And the response should contain json property "approved" with value false

Scenario: Void endpoint of the Bogus resource should not cancel a phony transaction, returning xml
      Given the following parameters
          | name         | value            |
          | amount       | 1000             |
          | ident        | 2                |
          | test         | true             |
          When I post to the Void endpoint of the Bogus resource as xml
          Then the response code should indicate unauthorized
          And the response should contain "<void>"
          And the response should contain "<approved>false</approved>"

Scenario: Authorize endpoint of the Bogus resource should only allow POST requests
        Given the following parameters
          | name         | value            |
          | fake         | param            |
          When I get the Authorize endpoint of the Bogus resource as json
          Then the response code should indicate that the method is not allowed

Scenario: Capture endpoint of the Bogus resource should only allow POST requests
        Given the following parameters
          | name         | value            |
          | fake         | param            |
          When I get the Capture endpoint of the Bogus resource as json
          Then the response code should indicate that the method is not allowed

Scenario: Void endpoint of the Bogus resource should only allow POST requests
        Given the following parameters
          | name         | value            |
          | fake         | param            |
          When I get the Void endpoint of the Bogus resource as json
          Then the response code should indicate that the method is not allowed

