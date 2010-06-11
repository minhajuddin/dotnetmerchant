# Filters added to this controller apply to all controllers in the application.
# Likewise, all the methods added will be available for all controllers.

class ApplicationController < ActionController::Base
  helper :all # include all helpers, all the time
  #protect_from_forgery # See ActionController::RequestForgeryProtection for details

  # Scrub sensitive parameters from your log
  # filter_parameter_logging :password

  def build_creditcard_from_params( params )
    ActiveMerchant::Billing::CreditCard.new(
            :number     => params[:number],
            :month      => params[:month],
            :year       => params[:year],
            :first_name => params[:first_name],
            :last_name  => params[:last_name],
            :verification_value => params[:verification]  )
  end
end
