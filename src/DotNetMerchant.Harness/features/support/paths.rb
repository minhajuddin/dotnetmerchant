module NavigationHelpers
  def path_to( path )
    #translate as needed
    resource = path[:resource]
    endpoint = path[:endpoint]
    format = path[:format]
    if resource == 'creditcard'
      resource = 'credit_card'
    end

    if resource != nil
	     '/' + resource.downcase + '/' + endpoint.downcase + ( format == nil ? '.' + format : '' )
    else
          if endpoint != nil
	        '/' + endpoint + ( format.length > 0 ? '.' + format : '' )
          else
            raise 'at least an endpoint must be specified.'
          end
      end
  end
end

World(NavigationHelpers)
