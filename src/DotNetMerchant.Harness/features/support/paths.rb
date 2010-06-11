module NavigationHelpers
  def path_to( path )

    resource = path[:resource]
    endpoint = path[:endpoint]
    format = path[:format]

    unless endpoint
      raise 'at least an endpoint must be specified.'
    end

    #translate as needed
    if resource && resource.downcase == 'creditcard'
      resource = 'credit_card'
    end

    if resource
	     '/' + resource.downcase + '/' + endpoint.downcase + ('.'  + format.downcase if format)
    else
	     '/' + endpoint + ('.'  + format.downcase if format)
    end
  end
end

World(NavigationHelpers)
