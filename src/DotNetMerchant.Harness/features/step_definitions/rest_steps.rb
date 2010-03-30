#jason's curl rest steps
require 'curb'
$root_url = "http://localhost:3000"

Given /^the following post parameters$/ do |table|
  table.hashes.each do |hash|
    Factory(:PostParam, hash)
  end
end

When /^(?:|I )get (.+)$/ do |endpoint|
  path = path_to(endpoint)
  url = $root_url + path
  c = Curl::Easy.http_get( url )
  @body = c.body_str
  @responseCode = c.response_code
end

When /^(?:|I )post to (.+)$/ do |endpoint|
  path = path_to(endpoint)
  url = $root_url + path
  req = Curl::Easy.new( url ) {
  PostParam.all do |param|
	Curl::PostField.new(param.name, param.value)
  end }
  req.http_post
end
When /^I restfully post to the VerifyCreditCard endpoint as xml$/ do
  pending # express the regexp above with the code you wish you had
end


Then /^(?:|the )response should contain xml element "([^\"]*)"$/ do |text|  
	assert @body.include? "<" + text + ">" 
end

Then /^the response should contain json property "([^\"]*)"$/ do |prop|
  assert @body.include? '"' + prop + '"'
end

Then /^the response code should indicate success$/ do
   assert @responseCode >= 200 and @responseCode < 300
end

  
