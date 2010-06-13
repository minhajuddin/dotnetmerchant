# rest steps
require 'rest_client'
require 'cgi'
$root_url = "http://localhost:3000"

Given /^the following parameters$/ do |table|
  @parameters = Hash.new
  table.hashes.each do |hash|
    @parameters[hash[:name]] = hash[:value]
  end
  #@parameters = table.hashes
end

When /^(?:|I )get the (\b.*\b) endpoint as (.+)$/ do |endpoint, format|
  path = path_to( :endpoint => endpoint, :format => format)
  print path
  url = $root_url + path
  first = true 

  if !@parameters.nil?
    @parameters.each do |param|
      url = url + (first ? '?' : '&' )
      first = false
      url = url + param[0] + '=' + CGI.escape(param[1].to_s)
    end
  end
  response = RestClient.get(url)
  @body = response.body
  @responsecode = response.code
end

When /^(?:|I )post to the (\b.*\b) endpoint as (.+)$/ do |endpoint, format|
  path = path_to(  :endpoint => endpoint, :format => format)
  url = $root_url + path
  begin
    response = RestClient.post( url, @parameters )
  rescue => x
   response = x.response
  end
  @body = response.body
  @responsecode = response.code
end

When /^(?:|I )post to the (\b.*\b) endpoint of the (\b.*\b) resource as (.+)$/ do |endpoint, resource, format|
  path = path_to(:resource => resource, :endpoint => endpoint, :format => format)
  url = $root_url + path
  begin
    response = RestClient.post( url, @parameters )
  rescue => x
    response = x.response
  end
  @body = response.body
  @responsecode = response.code
end

When /^(?:|I )get the (\b.*\b) endpoint of the (\b.*\b) resource as (.+)$/ do |endpoint, resource, format|

  path = path_to( :resource => resource, :endpoint => endpoint, :format => format)
  url = $root_url + path
  first = true
  if !@parameters.nil?
    @parameters.each do |param|
      url = url + (first ? '?' : '&' )
      first = false
      url = url + param[0] + '=' + CGI.escape(param[1].to_s)
    end
  end
  begin
    response = RestClient.get( url )
  rescue => x
    response =x.response
  end
  @body = response.body
  @responsecode = response.code
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
   assert @responsecode >= 200 and @responsecode < 300
end

Then /^the response code should indicate failure$/ do
   assert @responsecode >= 400
end

Then /^the response code should indicate unauthorized/ do
   assert @responsecode == 401
end