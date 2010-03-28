class VersionController < ApplicationController
  def index
    @version = Version.new(0,0,1)

	respond_to do |format|
		format.json{ render:json=>@version.to_json}
		format.xml 
	end
  end
end