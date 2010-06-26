require 'active_merchant'

class GatewayBaseController < ApplicationController

  def creategateway (options = {})
    test = options[:test] || false
    ActiveMerchant::Billing::Base.mode = test

    ActiveMerchant::Billing::BogusGateway.new({
            :login    => options[:login] || 'user',
            :password => options[:password] || 'password'
    })
  end


  def authorize
    if !request.post?
      render_post_required_error
      return
    end

    creditcard = build_creditcard_from_params(params)
    amount = params[:amount]
    gateway = creategateway(params)


    begin
      gateway_response = gateway.authorize(amount, creditcard)
      auth = GatewayResponse.new gateway_response
      render_result(auth, "authorization")
    rescue StandardError => x
      render_error(x, "authorization")
    end
  end


  def purchase
    if !request.post?
      render_post_required_error
      return
    end
    creditcard = build_creditcard_from_params(params)
    amount = params[:amount].to_i
    gateway = creategateway(params)

    begin
      gateway_response = gateway.purchase(amount, creditcard)
      purchase = GatewayResponse.new gateway_response
      render_result(purchase, "purchase")
    rescue StandardError => x
      f = Fakovv.new
      f.message = x.message << ' amount was ' << amount
      render_error(f, "purchase")
    end
  end

  def capture
    if !request.post?
      render_post_required_error
      return
    end

    amount = params[:amount]
    ident = params[:ident]

    gateway = creategateway(params)
    begin
      response = gateway.capture(amount, ident)
      capture = GatewayResponse.new response
      render_result(capture, "capture")
    rescue StandardError => x
      render_error(x, "capture")
    end
  end

  def void
    if !request.post?
      render_post_required_error
      return
    end

    gateway = creategateway(params)

    ident = params[:ident]
    begin
      response = gateway.void(ident)
      void = GatewayResponse.new response
      exclude = [:amount, :cvv_result, :avs_result]
      render_result(void, "void", {:except => exclude})
    rescue StandardError => x
      render_error(x, "void")
    end
  end

  def credit
    if !request.post?
      render_post_required_error
      return
    end
    amount = params[:amount]
    ident = params[:ident]

    gateway = creategateway(params)
    begin
      response = gateway.credit(amount, ident)
      credit = GatewayResponse.new response
      render_result(credit, 'credit')
    end
  rescue StandardError => x
    render_error(x, 'credit')
  end


  private
  def render_error(x, action)
    error = {:error => x.message}
    respond_to do |format|
      format.json do
        render :status => 400, :json => error.to_json
      end
      format.xml do
        render :status => 400, :xml => error.to_xml(:root => action, :skip_types => true)
      end
    end
  end

  def render_result(response, action, options={})
    respond_to do |format|
      format.json do
        render :status => (response.approved? ? 200 : 401), :json => response.to_json(options)
      end
      format.xml do
        options[:root] = action
        options[:dasherize] = false unless !options[:dasherize].nil?
        options[:skip_types] = true unless !options[:skip_types].nil?
        render :status => (response.approved? ? 200 : 401), :xml => response.to_xml(options)
      end
    end
  end

  
end

class Fakovv
  attr_accessor :message
end