module NavigationHelpers
  def path_to(resource = "", endpoint = "", format ="")
    #translate as needed
    resource.downcase!
    endpoint.downcase!
    format.downcase!
    if resource == 'creditcard'
      resource = 'credit_card'
    end

    if resource.length > 0
	     '/' + resource + '/' + endpoint + ( format.length > 0 ? '.' + format : "" )
      else
          if endpoint.length > 0
	        '/' + endpoint + '.' + ( format.length > 0 ? '.' + format : "" )
          else
            raise 'at least an endpoint must be specified.'
          end
      end
  end
end

World(NavigationHelpers)
