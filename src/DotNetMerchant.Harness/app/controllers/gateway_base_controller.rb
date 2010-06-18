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
    if !request.post?
      render_post_required_error
      return
    end

    @creditcard = build_creditcard_from_params(params)
    @amount = params[:amount]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]

    @gateway_response =  gateway.authorize(@amount, @creditcard)
    if @gateway_response.success?
      @ident = @gateway_response.authorization
      @approved = true
      @msg = ""
    else
      @approved = false
      @msg = @gateway_response.message
    end
    respond_to do |format|
      format.json do
        render :status => (@approved ? 200 : 401), :json =>
                {
                        :credit_card => @creditcard,
                        :approved => @approved,
                        :amount => @amount,
                        :ident => @ident,
                        :message => @msg,
                        :cvv_result => @gateway_response.cvv_result,
                        :avs_result => @gateway_response.avs_result
                }.to_json
      end
      format.xml do
        render :status => (@approved ? 200 : 401)
      end
    end
  end


  def purchase
    if !request.post?
      render_post_required_error
      return
    end
    @creditcard = build_creditcard_from_params(params)
    @amount = params[:amount]
    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end

    @gateway_response = gateway.purchase(@amount, @creditcard)
    if @gateway_response.success?
      @ident = @gateway_response.authorization
      @approved = true
      @msg = ""
    else
      @approved = false
      @msg = @gateway_response.message
    end
    respond_to do |format|
      format.json do
         render :status => (@approved ? 200 : 401), :json =>
                {
                        :credit_card => @creditcard,
                        :approved => @approved,
                        :amount => @amount,
                        :ident => @ident,
                        :message => @msg,
                        :cvv_result => @gateway_response.cvv_result,
                        :avs_result => @gateway_response.avs_result
                }.to_json
      end
      format.xml do
        render :status => (@approved ? 200 : 401)
      end
    end
  end

  def capture
    if !request.post?
      render_post_required_error
      return
    end
    @amount = params[:amount]
    @ident = params[:ident]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]

    @ident = params[:ident]
    response = gateway.capture(@amount, @ident)
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
    if !request.post?
      render_post_required_error
      return
    end
    @ident = params[:ident]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]

    @ident = params[:ident]
    response = gateway.void(@ident)
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
    if !request.post?
      render_post_required_error
      return
    end
    @amount = params[:amount]
    @ident = params[:ident]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]

    @ident = params[:ident]
    response = gateway.credit(@amount, @ident)
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
