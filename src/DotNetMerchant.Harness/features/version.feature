Feature: Version API
	In order to get the current version
	As a REST Client
	I want to be able to send a get request and get back the version
	
	Scenario: Version endpoint should exist as xml
		When I restfully get the version endpoint as xml
		Then the response should contain xml element "version"

	Scenario: Version endpoint should exist as json
		When I restfully get the version endpoint as json
		Then the response should contain json property "major"
	