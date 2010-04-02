xml.instruct! :xml, :version => "1.0"
xml.creditcardvalidation do
	xml.valid @valid
	xml.creditcard do
		xml.number @credit_card.number
		xml.first_name @credit_card.first_name
		xml.last_name @credit_card.last_name
		xml.month @credit_card.month
		xml.year @credit_card.year
		
		xml.verification_number @credit_card.verification_value
		if ( !@valid )
			xml.errors do
				@credit_card.errors.each do |err|
					xml.error err 
				end
			end
		end
	end
end
