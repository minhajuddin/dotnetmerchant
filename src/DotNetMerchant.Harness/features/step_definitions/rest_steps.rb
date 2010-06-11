#jason's curl rest steps
require 'curb'
$root_url = "http://localhost:3000"

Given /^the following parameters$/ do |table|
  table.hashes.each do |hash|
    Factory(:PostParam, hash)
  end
end

When /^(?:|I )get the (\b.*\b) endpoint as (.+)$/ do |endpoint, format|
  path = path_to( :endpoint => endpoint, :format => format)
  print path
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
  @body = c.body_str
  @responseCode = c.response_code
end

When /^(?:|I )post to the (\b.*\b) endpoint as (.+)$/ do |endpoint, format|
  path = path_to(  :endpoint => endpoint, :format => format)
  url = $root_url + path
  req = Curl::Easy.new( url ) {
  PostParam.all do |param|
	Curl::PostField.new(param.name, param.value)
  end }
  req.http_post
  @responseCode = req.response_code
end

When /^(?:|I )post to the (\b.*\b) endpoint of the (\b.*\b) resource as (.+)$/ do |endpoint, resource, format|
  path = path_to(:resource => resource, :endpoint => endpoint, :format => format)
  url = $root_url + path
  req = Curl::Easy.new( url ) {
  PostParam.all do |param|
	Curl::PostField.new(param.name, param.value)
  end }
  req.http_post
  @responseCode = req.response_code
end

When /^(?:|I )get the (\b.*\b) endpoint of the (\b.*\b) resource as (.+)$/ do |endpoint, resource, format|

  path = path_to( :resource => resource, :endpoint => endpoint, :format => format)
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

  @body = c.body_str
  @responseCode = c.response_code
end


Then /^(?:|the )response should contain xml element "([^\"]*)"$/ do |text|  
	assert @body.include? "<" + text + ">" 
end

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

