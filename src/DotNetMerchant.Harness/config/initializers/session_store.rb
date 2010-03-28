# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key         => '_leafblower_session',
  :secret      => 'd0dda335a7d96cf996c4d1a5668a5743c1f70b674fa935897cb5b57c02a7723d0f9677bfe15f7244b9d7613f4b79421d4a6f08af67b3f54e8939585a64aa19be'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
