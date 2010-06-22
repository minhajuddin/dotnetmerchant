class BraintreeController < GatewayBaseController

  def creategateway (options = {})
    test = options[:test] || false
    ActiveMerchant::Billing::Base.mode = test

    ActiveMerchant::Billing::BraintreeGateway.new({
            :login    => options[:login] || 'user',
            :password => options[:password] || 'password'
    })
  end
end