<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="admin_wrapper">
	<h1>Super Simple Admin Theme</h1>
	
	<p>Welcome. To log in just hit the log in button below.</p>
	
	<div class="attention">For the demo, just hit the login button</div>
	
	<form action="index1.html" method="get">
		<p><label>Username</label>
	  	<input name="username" type="text" class="input large" value="username" />
	  </p>
		
		<p><label>Password</label>
		<input name="password" type="password" class="input large" value="password" />
	  </p>
	  
	  <p><input type="submit" name="Submit" id="button" value="Login"  class="button"/>
		<input type="reset" name="reset" id="reset" value="Reset" class="button" />
		</p>
	</form>
</div>

