xml.instruct! :xml, :version => "1.0"
if ( !@error.nil?)
  xml.error @error
else
    xml.authorization do
      xml.approved @approved
      if @approved
        xml.ident @gateway_response.authorization
        xml.amount @gateway_response.params["authorized_amount"] unless @gateway_response.params["authorized_amount"].nil?
      end
      xml.message @gateway_response.message
      if !@gateway_response.cvv_result.nil?
        xml.cvv_result do
           xml.code @gateway_response.cvv_result["code"]
           xml.message @gateway_response.cvv_result["message"]
        end
      end
      if !@gateway_response.avs_result.nil?
        xml.avs_result do
          xml.code @gateway_response.avs_result["code"]
          xml.postal_match @gateway_response.avs_result["postal_match"]
          xml.street_match @gateway_response.avs_result["street_match"]
          xml.message @gateway_response.avs_result["message"]
        end
      end
      xml.creditcard do
        xml.number @creditcard.number
        xml.first_name @creditcard.first_name
        xml.last_name @creditcard.last_name
        xml.month @creditcard.month
        xml.year @creditcard.year
        xml.verification_number @creditcard.verification_value
      end
    end
end
