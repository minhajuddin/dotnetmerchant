xml.instruct! :xml, :version => "1.0"
xml.authorization do
  xml.approved @approved
  if @approved
    xml.ident @ident
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
