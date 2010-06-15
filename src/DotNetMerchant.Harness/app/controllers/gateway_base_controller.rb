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
    if !ensure_post?
        return
    else
      @creditcard = build_creditcard_from_params( params )
      @amount = params[:amount]
      if params[:test]
        ActiveMerchant::Billing::Base.mode = :test
      end

      response = gateway.authorize( @amount, @creditcard )
      if response.success?
        @ident = response.authorization
        @approved = true
        @msg = ""
      else
        @approved = false
        @msg = response.message
      end
      respond_to do |format|
        format.json do
          render :status => (@approved ? 200 : 401), :json=>{:credit_card => @creditcard, :approved => @approved, :amount => @amount, :ident => @ident, :message => @msg}.to_json
        end
        format.xml do
          render :status => (@approved ? 200 : 401)
        end
      end
    end
  end


  def purchase
    if !ensure_post?
        return
    else
      @creditcard = build_creditcard_from_params( params )
      @amount = params[:amount]
      if params[:test]
        ActiveMerchant::Billing::Base.mode = :test
      end

      response = gateway.purchase( @amount, @creditcard )
      if response.success?
        @ident = response.authorization
        @approved = true
        @msg = ""
      else
        @approved = false
        @msg = response.message
      end
      respond_to do |format|
        format.json do
          render :status => (@approved ? 200 : 401), :json=>{:credit_card => @creditcard, :approved => @approved, :amount => @amount, :ident => @ident, :message => @msg}.to_json
        end
        format.xml do
          render :status => (@approved ? 200 : 401)
        end
      end
    end
  end

  def capture
    if !ensure_post?
      return
    end
    @amount = params[:amount]
    @ident = params[:ident]
    
    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end
    @ident = params[:ident]
    response = gateway.capture( @amount, @ident )
    @msg = response.message
    if response.success?
        @approved = true
      else
        @approved = false
      end
      respond_to do |format|
        format.json do
          render :status => (@approved ? 200 : 401), :json=>{:approved => @approved, :amount => @amount, :ident => @ident, :message => @msg}.to_json
        end
        format.xml do
          render :status => (@approved ? 200 : 401)
        end
    end
  end

  def void
    if !ensure_post?
      return
    end
    @ident = params[:ident]

    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end
    @ident = params[:ident]
    response = gateway.void( @ident )
    @msg = response.message
    if response.success?
        @approved = true
      else
        @approved = false
      end
      respond_to do |format|
        format.json do
          render :status => (@approved ? 200 : 401), :json=>{:approved => @approved, :ident => @ident, :message => @msg}.to_json
        end
        format.xml do
          render :status => (@approved ? 200 : 401)
        end
    end
  end

  def credit
    if !ensure_post?
      return
    end
    @amount = params[:amount]
    @ident = params[:ident]

    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end
    @ident = params[:ident]
    response = gateway.credit( @amount, @ident )
    @msg = response.message
    if response.success?
        @approved = true
      else
        @approved = false
      end
      respond_to do |format|
        format.json do
          render :status => (@approved ? 200 : 401), :json=>{:approved => @approved, :amount => @amount, :ident => @ident, :message => @msg}.to_json
        end
        format.xml do
          render :status => (@approved ? 200 : 401)
        end
    end
  end


end
