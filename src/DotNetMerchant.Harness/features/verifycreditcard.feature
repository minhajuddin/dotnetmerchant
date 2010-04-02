Feature: VerifyCreditCard API
	In order to validate a credit card
	As a REST Client
	I want to be able to post credit card details and get back a status

Scenario: VerifyCreditCard endpoint should validate a valid card and return xml
		Given the following parameters
		| name         | value            |
		| number       | 4111111111111111 |
		| month        | 8                |
		| year         | 2011             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I get the VerifyCreditCard endpoint as xml
		Then the response code should indicate success 
		And the response should contain "<valid>true</valid>"

		
Scenario: VerifyCreditCard endpoint should validate a valid card and return json
		Given the following parameters
		| name         | value            |
		| number       | 4111111111111111 |
		| month        | 8                |
		| year         | 2011             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I get the VerifyCreditCard endpoint as json
		Then the response code should indicate success 
		And the response should contain json property "valid" with value true
		
Scenario: VerifyCreditCard endpoint should not validate an invalid card returning json
		Given the following parameters
		| name         | value            |
		| number       | 4111111111111112 |
		| month        | 8                |
		| year         | 2011             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I get the VerifyCreditCard endpoint as json
		Then the response code should indicate failure 
		And the response should contain json property "valid" with value false	

Scenario: VerifyCreditCard endpoint should not validate an invalid card returning xml
		Given the following parameters
		| name         | value            |
		| number       | 4111111111111112 |
		| month        | 8                |
		| year         | 2011             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I get the VerifyCreditCard endpoint as xml
		Then the response code should indicate failure 
		And the response should contain "<valid>false</valid>"
		
Scenario: VerifyCreditCard endpoint should not validate an invalid card returning json
		Given the following parameters
		| name         | value            |
		| number       | 4111111111111111 |
		| month        | 1                |
		| year         | 2010             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I get the VerifyCreditCard endpoint as json
		Then the response code should indicate failure 
		And the response should contain json property "valid" with value false	
		And the response should contain "expired"

Scenario: VerifyCreditCard endpoint should not validate an expired card returning
		Given the following parameters
		| name         | value            |
		| number       | 4111111111111111 |
		| month        | 1                |
		| year         | 2010             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I get the VerifyCreditCard endpoint as xml
		Then the response code should indicate failure 
		And the response should contain "<valid>false</valid>"
		And the response should contain "expired"		