Feature: VerifyCreditCard API
	In order to validate a credit card
	As a REST Client
	I want to be able to post credit card details and get back a status

Scenario: VerifyCreditCard endpoint should exist as xml
		Given the following post parameters
		| name         | value            |
		| number       | 4111111111111111 |
		| month        | 8                |
		| year         | 2011             |
		| first_name   | Santos L         |
		| last_name    | Halper           | 
		| verification | 121              |
		When I post to the VerifyCreditCard endpoint as xml
		Then the response code should indicate success 
