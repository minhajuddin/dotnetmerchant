Feature: Version API
	In order to get the current version
	As a REST Client
	I want to be able to send a get request and get back the version
	
	Scenario: Version endpoint should exist as xml
		When I get the version endpoint as xml
		Then the response code should indicate success 
		And the response should contain xml element "version"
		And the response should contain xml element "major"
		And the response should contain xml element "minor"
		And the response should contain xml element "revision"

	Scenario: Version endpoint should exist as json
		When I get the version endpoint as json
		Then the response code should indicate success
		And the response should contain json property "major"
		And the response should contain json property "minor"
		And the response should contain json property "revision"
	