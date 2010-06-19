require 'active_merchant'

class GatewayResponse

  def initialize (from_response)
    @hash = {
            :ident => from_response.authorization,
            :amount => from_response.params["authorized_amount"],
            :approved => from_response.success?,
            :msg => from_response.message,
            :cvv_result => from_response.cvv_result,
            :avs_result => from_response.avs_result
    }
  end

  def approved?
    if hash[:approved] == true
      print 'returning true from approved?'
      true
    else
      print 'returning false from approved?'
      false
    end
  end

  def to_xml(options = {})
    @hash.to_xml(options)
  end

  def to_json(options = {})
    @hash.to_json(options)
  end
end
