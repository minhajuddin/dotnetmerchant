Feature: VerifyCreditCard API
	In order to validate a credit card
	As a REST Client
	I want to be able to post credit card details and get back a status

Scenario: VerifyCreditCard endpoint should exist as xml
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

		
Scenario: VerifyCreditCard endpoint should exist as json
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