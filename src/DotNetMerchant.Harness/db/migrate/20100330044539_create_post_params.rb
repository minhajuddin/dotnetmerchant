class CreatePostParams < ActiveRecord::Migration
  def self.up
    create_table :post_params do |t|
      t.string :name
      t.string :value

      t.timestamps
    end
  end

  def self.down
    drop_table :post_params
  end
end
