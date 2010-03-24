# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key         => '_DotNetMerchant.Harness_session',
  :secret      => 'b3adcd94e7b49dcfe6a009b0e9c6da3a24be4f4ac3e36ec748b50558b1d05899474930b36d60f1f856a8de468e46c196edc1473a9c2ea4e57a33e061cbe2e642'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
