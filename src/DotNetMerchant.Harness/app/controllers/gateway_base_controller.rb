require 'active_merchant'
class GatewayBaseController < ApplicationController

  def new

  end

  def gateway
    if @_gateway.nil?
       @_gateway = ActiveMerchant::Billing::BogusGateway.new({
       :login    => 'demo',
       :password => 'password'
    })
    end
    return @_gateway
  end
  def authorize
    @creditcard = build_creditcard_from_params( params )
    @amount = params[:amount]
    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end

    response = gateway.authorize( @amount, @creditcard )
    if response.success?
      @ident = response.authorization
      @approved = true
    else
      @approved = false
    end
    respond_to do |format|
      format.json do
        render :status => (@approved ? 200 : 401), :json=>{:credit_card => @creditcard, :approved => @approved, :amount => @amount, :ident => @ident}.to_json
      end
      format.xml do
        render :status => (@approved ? 200 : 401)
      end
    end
  end

  def capture
    @creditcard = build_creditcard_from_params( params )
    @amount = params[:amount]
    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end
    @ident = params[:ident]
    gateway.capture( @amount, @creditcard, @ident )
  end

  def void
  end

  def credit
  end


end
