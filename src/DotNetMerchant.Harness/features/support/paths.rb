module NavigationHelpers
  def path_to( path )
    #translate as needed
    resource = path[:resource]
    endpoint = path[:endpoint]
    format = path[:format]

    if resource && resource.downcase == 'creditcard'
      resource = 'credit_card'
    end

    if resource
	     '/' + resource.downcase + '/' + endpoint.downcase + ( '.'  + format.downcase if format)
    else
          if endpoint
	        '/' + endpoint + ('.'  + format.downcase if format)
          else
            raise 'at least an endpoint must be specified.'
          end
      end
  end
end

World(NavigationHelpers)
