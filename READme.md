
# Welcome to the My John Deere OAuth2 CSharp Example. 
  
This project gives a full native CSharp example of:
 * Getting a OAuth2 access token
 * Call the MyJohnDeere APIs with your access token  
 * Edit Settings  
     
## Requirements  
* ASP.NET Core (Current Version: 3.1)
* Installed NuGet Packages:
   * Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation Version="3.0.0"
   * Microsoft.Extensions.Logging.Debug Version="3.0.0" 
   * Microsoft.VisualStudio.Web.CodeGeneration.Design Version="3.0.0"
   * Newtonsoft.Json Version="12.0.3"
* A free port 9090 (you can change port in: Project Properties-Debug-then App URL).
* The callback for your application needs to be configured for the URL of this app:
	* In developer.deere.com, add [http://localhost:9090/callback](http://localhost:9090/callback) as one of the callbacks within OAuth Profile (you can have more than one).
	* It is NOT recommended to keep this around for production use.
	* You use other ports/URLs and will also needs to be registered in [https://developer.deere.com](https://developer.deere.com/). (Please allow up to 20 minutes for any changes to be replicated).
  
## Edit Settings 
* Once in the browser you will need a few things (Alternative option: update appsettings.json file directly) 
  * A ClientId and Secret from your application on https://developer.deere.com
* Add Scopes
  * Insert scopes, for example org1 org2. Can also be found under MyJohnDeere API Documentation section within [https://developer.deere.com](https://developer.deere.com/) for each endpoint.

## How to start this project  
* Clone this repository:  
  * ```git clone git@github.com:JohnDeere/MyJohnDeereAPI-OAuth2-CSharp-Example.git```  
* Build the project  
  * ```Build Solution```  
* Start it  
  * ```Run IIS Express```  