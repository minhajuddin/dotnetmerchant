xml.instruct! :xml, :version => "1.0"
if ( !@error.nil?)
  xml.error @error
else
    xml.void do
      xml.approved @approved
      xml.message @msg
      if @approved
        xml.ident @ident
      end
    end
end
