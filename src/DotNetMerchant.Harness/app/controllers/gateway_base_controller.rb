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

    creditcard = build_creditcard_from_params(params)
    amount = params[:amount]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]

    gateway_response = gateway.authorize(amount, creditcard)
    auth = GatewayResponse.new gateway_response

    respond_to do |format|
      format.json do
        render :status => ( auth.approved? ? 200 : 401), :json => auth.to_json
      end
      format.xml do
        render :status => ( auth.approved? ? 200 : 401), :xml => auth.to_xml( :root => 'authorization', :dasherize => false, :skip_types => true )
      end
    end
  end


  def purchase
    if !request.post?
      render_post_required_error
      return
    end
    creditcard = build_creditcard_from_params(params)
    amount = params[:amount]
    if params[:test]
      ActiveMerchant::Billing::Base.mode = :test
    end

    gateway_response = gateway.purchase(amount, creditcard)
    purchase = GatewayResponse.new gateway_response

    if purchase.approved?
      status = 200
    else
      status = 401
    end

    respond_to do |format|
      format.json do
         render :status => status, :json => purchase.to_json
      end
      format.xml do
        render :status => status, :xml => purchase.to_xml( :root => "purchase", :dasherize => false, :skip_types => true)
      end
    end
  end

  def capture
    if !request.post?
      render_post_required_error
      return
    end
    amount = params[:amount]
    ident = params[:ident]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]


    response = gateway.capture(amount, ident)
    capture = GatewayResponse.new response

    respond_to do |format|
      format.json do
        render :status => (capture.approved? ? 200 : 401), :json => capture.to_json
      end
      format.xml do
        render :status => (capture.approved? ? 200 : 401), :xml => capture.to_xml(:root => "capture", :dasherize => false, :skip_types => true)
      end
    end
  end

  def void
    if !request.post?
      render_post_required_error
      return
    end

    ActiveMerchant::Billing::Base.mode = :test if params[:test]

    ident = params[:ident]
    response = gateway.void(ident)
    void = GatewayResponse.new response
    exclude = [:amount, :cvv_result, :avs_result]
    respond_to do |format|
      format.json do
        render :status => (void.approved? ? 200 : 401), :json => void.to_json(:except => exclude)
      end
      format.xml do
        render :status => (void.approved? ? 200 : 401), :xml =>  void.to_xml(:except => exclude, :root =>'void', :dasherize => false, :skip_types => true)
      end
    end
  end

  def credit
    if !request.post?
      render_post_required_error
      return
    end
    amount = params[:amount]
    ident = params[:ident]

    ActiveMerchant::Billing::Base.mode = :test if params[:test]
    response = gateway.credit(amount, ident)
    credit = GatewayResponse.new response
    respond_to do |format|
      format.json do
        render :status => (credit.approved? ? 200 : 401), :json => credit.to_json
      end
      format.xml do
        render :status => (credit.approved? ? 200 : 401), :xml => credit.to_xml(:root=>'credit', :dasherize => false, :skip_types => true)
      end
    end
  end


end
