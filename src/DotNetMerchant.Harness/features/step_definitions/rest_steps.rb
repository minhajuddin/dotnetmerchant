#jason's curl rest steps
require 'curb'
$root_url = "http://localhost:3000"

When /^(?:|I )restfully get (.+)$/ do |endpoint|
  path = path_to(endpoint)
  url = $root_url + path
  c = Curl::Easy.http_get( url )
  @body = c.body_str
  @responseCode = c.response_code
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
  
