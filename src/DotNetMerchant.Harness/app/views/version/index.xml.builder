xml.instruct! :xml, :version => "1.0"
xml.version do
	xml.major @version.major
	xml.minor @version.minor
	xml.revision @version.revision
end
