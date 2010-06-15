xml.instruct! :xml, :version => "1.0"
if ( !@error.nil?)
  xml.error @error
else
    xml.credit do
      xml.approved @approved
      xml.message @msg
      xml.amount @amount
      if @approved
        xml.ident @ident
      end
    end
end
