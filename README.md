# otohits-api-csharp

C# implementation of the Otohits API (http://docs.otohitsapi.apiary.io).

## Quick start as fast as possible
You can use the Otohits.API.TestConsole project to play with the API.
Add your API Key/Secret in the `App.Config` file:

```
<appSettings>
  <add key="Otohits:API:Key" value="yourApiKey"/>
  <add key="Otohits:API:Secret" value="yourSecret"/>
</appSettings>
```

And launch the Console App! You should get your account info.


## How to use

Set your credentials once to make all furthers request
```
OtohitsRequest.SetCredentials("yourApiKey", "yourSecret");
```

### Make a request

#### Using a mapped method:
```
var user = new OtohitsRequest().GetUserInfo();
```

#### Custom call
Most of the services are directly mapped into `OtohitsRequest`, but if you need to make a specific request, you can use `Get` or `Post` methods.
Both can retrieve either a JSON string or a POCO object (JSON deserialize).
Basic `Get` and `Post` also have Async() methods.

You can use the POCO object if available
```
 var user = new OtohitsRequest().Get<ApiResponse<User>>("/me");
```

Or parse the object on the JSON response on your side if needed:
```
string userResponse = new OtohitsRequest().Get("/me");
```

### APIResponse
All the response coming from the API is returning an object with the status (success), the data (data...) and errors if any.
```
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public List<ApiError> Errors { get; set; }
}
```
