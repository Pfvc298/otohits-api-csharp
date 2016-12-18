# Otohits API - C&#35;

C# implementation of the Otohits API (http://docs.otohitsapi.apiary.io).

## Quick start
You can use the Otohits.API.TestConsole project to play with the API.

Add your API Key/Secret in the `App.Config` file:

```xml
<appSettings>
  <add key="Otohits:API:Key" value="yourApiKey"/>
  <add key="Otohits:API:Secret" value="yourSecret"/>
</appSettings>
```

Launch the Console App and... That's it! You should get your account info.


## How to use

Set your credentials once to make all furthers request
```cs
OtohitsRequest.SetCredentials("yourApiKey", "yourSecret");
```

### Make a request

#### Using a mapped method
```cs
var user = new OtohitsRequest().GetUserInfo();
```

#### Using a custom call
Most of the services are directly mapped into `OtohitsRequest`, but if you need to make a specific request, you can use `Get`, `Post`, `PUT` and `DELETE` methods.

Both can retrieve either a JSON string or a POCO object (JSON deserialize).

Basic `Get`, `Post`, `PUT` and `DELETE` methods also have Async() methods.

You can use the POCO object if available
```cs
 var user = new OtohitsRequest().Get<ApiResponse<User>>("/me");
```

Or just get back the JSON string if needed:
```cs
string userResponse = new OtohitsRequest().Get("/me");
```

### APIResponse
All the response coming from the API return an object with the status (success), the data (data...) and errors if any.
```cs
public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public List<ApiError> Errors { get; set; }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public List<ApiError> Errors { get; set; }
}
```


KISS :)
