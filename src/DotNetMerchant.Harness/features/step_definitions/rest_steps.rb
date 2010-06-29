# rest steps
require 'rest_client'
require 'cgi'
require 'yaml'

$root_url = "http://localhost:3000"
$credfilepath = File.join(Rails.root, 'features', 'support', 'credentials.yml')

Given /^the following parameters$/ do |table|
  @parameters = Hash.new
  table.hashes.each do |hash|
    @parameters[hash[:name]] = hash[:value]
  end
  #@parameters = table.hashes
end

Given /^the (\b.*\b) credentials$/ do |gateway|
   

   assert File.exists?($credfilepath), "Add a file called credentials.yml to features/support. see credentials.yml.example"

   credfile = YAML.load_file($credfilepath)

   assert !credfile[gateway].nil?, "Add credentials called '" + gateway + "' to features/support/credentials.yml"
   assert !credfile[gateway]["credentials"].nil?, "Add values for credentials: to gateway " + gateway + " in features/support/credentials.yml"
   
   credfile[gateway]["credentials"].each do | name, value |
     @parameters[name.intern] = value
   end
end

Given /^the (\b.*\b) gateway's (\b.*\b) (\b.*\b) details$/ do |gateway, variant, cardtype|

   assert File.exists?($credfilepath), "Add a file called credentials.yml to features/support. see credentials.yml.example"

   credfile = YAML.load_file($credfilepath)
   assert !credfile[gateway].nil?, "Add credentials called '" + gateway + "' to features/support/credentials.yml"
   assert !credfile[gateway][cardtype].nil?, "Add a card type called '" + cardtype + "' under the " + gateway + " gateway in features/support/credentials.yml"
   assert !credfile[gateway][cardtype][variant].nil?, "Add a card variant called '"+ variant + "' to card type " + cardtype + " under the " + gateway + " gateway in features/support/credentials.yml"
   credfile[gateway][cardtype][variant].each do | name, value |
     @parameters[name.intern] = value
   end

end


When /^(?:|I )get the (\b.*\b) endpoint as (.+)$/ do |endpoint, format|
  path = path_to( :endpoint => endpoint, :format => format)
  
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

Then /^the response code should indicate that the method is not allowed/ do
   assert @responsecode == 405
end