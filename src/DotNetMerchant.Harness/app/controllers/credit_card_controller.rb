require 'rubygems'
require 'active_merchant'

class CreditCardController < ApplicationController
  #GET /credit_card/verify.json
  #GET /credit_card/verify.xml
  def verify
    @credit_card = build_creditcard_from_params( params )
    
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
