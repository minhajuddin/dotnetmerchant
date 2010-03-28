#jason's curl rest steps
require 'curb'
$root_url = "http://localhost:3000"

When /^(?:|I )restfully get (.+)$/ do |endpoint|
  url = $root_url << path_to(endpoint)
  c = Curl::Easy.http_get( url )
  @body = c.body_str
end


Then /^(?:|the )response should contain xml element "([^\"]*)"$/ do |text|  
	assert @body.include? "<" << text << ">" 
end

Then /^the response should contain json property "([^\"]*)"$/ do |prop|
  assert @body.include? '"' << prop << '"'
end
