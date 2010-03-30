require 'active_merchant'

class VerifyCreditCardController < ApplicationController
#POST /verifycreditcard
#POST /verifycreditcard.xml
protect_from_forgery :only => [:create, :update, :destroy] 
def index
    credit_card = ActiveMerchant::Billing::CreditCard.new(
		:number     => params[:number],
		:month      => params[:month],
		:year       => params[:year],
		:first_name => params[:first_name],
		:last_name  => params[:last_name],
		:verification_value => params[:verification]	
		)
	@valid = credit_card.valid?
	
	respond_to do |format|
		format.json{ render:json=>@valid.to_json}
		format.xml 
	end
  end
end
