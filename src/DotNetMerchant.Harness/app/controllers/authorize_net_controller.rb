class AuthorizeNetController < GatewayBaseController
 def creategateway (options = {})
    test = options[:test] || false
    ActiveMerchant::Billing::Base.mode = :test if test

    ActiveMerchant::Billing::AuthorizeNetGateway.new({
            :login    => options[:login] || 'user',
            :password => options[:password] || 'password',
            :test => test
    })
  end
end