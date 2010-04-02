#jason's curl rest steps
require 'curb'
$root_url = "http://localhost:3000"

<<<<<<< HEAD:src/DotNetMerchant.Harness/features/step_definitions/rest_steps.rb
Given /^the following parameters$/ do |table|
  table.hashes.each do |hash|
    Factory(:PostParam, hash)
  end
end

When /^(?:|I )get (.+)$/ do |endpoint|
  path = path_to(endpoint)
  url = $root_url + path
  first = true 
  c = Curl::Easy.new
  PostParam.all.each do |param|
    url = url + (first ? '?' : '&' )
	first = false
	url = url + param.name + '=' + c.escape(param.value.to_s)
  end 
  c.url = url
  c.http_get()
=======
When /^(?:|I )restfully get (.+)$/ do |endpoint|
  path = path_to(endpoint)
  url = $root_url + path
  c = Curl::Easy.http_get( url )
>>>>>>> ea800715a1906ab57c328dc1f4a15f7ad0c42273:src/DotNetMerchant.Harness/features/step_definitions/rest_steps.rb
  @body = c.body_str
  @responseCode = c.response_code
end

<<<<<<< HEAD:src/DotNetMerchant.Harness/features/step_definitions/rest_steps.rb
When /^(?:|I )post to (.+)$/ do |endpoint|
  path = path_to(endpoint)
  url = $root_url + path
  req = Curl::Easy.new( url ) {
  PostParam.all do |param|
	Curl::PostField.new(param.name, param.value)
  end }
  req.http_post
  @responseCode = req.response_code
end
=======
>>>>>>> ea800715a1906ab57c328dc1f4a15f7ad0c42273:src/DotNetMerchant.Harness/features/step_definitions/rest_steps.rb

Then /^(?:|the )response should contain xml element "([^\"]*)"$/ do |text|  
	assert @body.include? "<" + text + ">" 
end

<<<<<<< HEAD:src/DotNetMerchant.Harness/features/step_definitions/rest_steps.rb
Then /^the response should contain json property "([^\"]*)" with value "([^\"]*)"$/ do |prop, value|
  json = ActiveSupport::JSON.decode(@body)
  json[prop].should == value
end

Then /^the response should contain json property "([^\"]*)" with value true$/ do |prop|
  json = ActiveSupport::JSON.decode(@body)
  assert json[prop]
end

Then /^the response should contain json property "([^\"]*)" with value false$/ do |prop|
  json = ActiveSupport::JSON.decode(@body)
  assert !json[prop]
end

Then /^the response should contain json property "([^\"]*)"$/ do |prop|
  json = ActiveSupport::JSON.decode(@body)
  assert !json[prop].nil?
end

Then /^the response should contain "([^\"]*)"$/ do |str|
  assert @body.include? str
end


Then /^the response code should indicate success$/ do
   assert @responseCode >= 200 and @responseCode < 300
end

Then /^the response code should indicate failure$/ do
   assert @responseCode >= 400
end

  
=======
Then /^the response should contain json property "([^\"]*)"$/ do |prop|
  assert @body.include? '"' + prop + '"'
end

Then /^the response code should indicate success$/ do
   assert @responseCode >= 200 and @responseCode < 300
end
  
>>>>>>> ea800715a1906ab57c328dc1f4a15f7ad0c42273:src/DotNetMerchant.Harness/features/step_definitions/rest_steps.rb
