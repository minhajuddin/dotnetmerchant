require 'rubygems'
require 'active_merchant'

class CreditCardController < ApplicationController
  #GET /credit_card/verify.json
  #GET /credit_card/verify.xml
  def verify
    @credit_card = ActiveMerchant::Billing::CreditCard.new(
            :number     => params[:number],
            :month      => params[:month],
            :year       => params[:year],
            :first_name => params[:first_name],
            :last_name  => params[:last_name],
            :verification_value => params[:verification]
    )
    if @credit_card.valid?
      @valid = true
    else
      @valid = false
    end
    respond_to do |format|
      format.json do
        render :status => (@valid ? 200 : 400), :json=>{:credit_card => @credit_card, :valid => @valid}.to_json
      end
      format.xml do
        render :status => (@valid ? 200 : 400)
      end
    end
  end
end	
