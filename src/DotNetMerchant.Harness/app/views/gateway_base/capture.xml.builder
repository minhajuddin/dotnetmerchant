xml.instruct! :xml, :version => "1.0"
if ( !@error.nil?)
  xml.error @error
else
    xml.capture do
      xml.approved @approved
      xml.message @msg
      if @approved
        xml.ident @ident
        xml.amount @amount
      end
    end
end
