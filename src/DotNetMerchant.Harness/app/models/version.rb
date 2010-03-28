class Version
 attr_accessor :major, :minor, :revision
 def initialize(major, minor, revision)
	@major = major
	@minor = minor
	@revision = revision
 end
 
 def to_s
	@major.to_s + '.' + @minor.to_s + '.' + @revision.to_s
 end
end