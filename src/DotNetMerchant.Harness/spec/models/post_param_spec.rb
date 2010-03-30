require 'spec_helper'

describe PostParam do
  before(:each) do
    @valid_attributes = {
      :name => "value for name",
      :value => "value for value"
    }
  end

  it "should create a new instance given valid attributes" do
    PostParam.create!(@valid_attributes)
  end
end
