class BraintreeController < GatewayBaseController

  def creategateway (test)
    ActiveMerchant::Billing::Base.mode = :test if test
    ActiveMerchant::Billing::BraintreeGateway.new({
            :login    => 'demo',
            :password => 'password'
    })
  end
end