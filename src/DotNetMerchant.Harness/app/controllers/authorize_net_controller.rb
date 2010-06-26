class AuthorizeNetController < GatewayBaseController
 def creategateway (options = {})
    test = options[:test] || false
    ActiveMerchant::Billing::Base.mode = test ? :test : :production

    ActiveMerchant::Billing::AuthorizeNetGateway.new({
            :login    => options[:login] || 'user',
            :password => options[:password] || 'password',
            :test => options[:gatewaytest] || false
    })
  end
end