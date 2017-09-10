![Logo](../../favicon.ico)
# Authorize by Attribute

> Project Created by [**Top Nguyen**](http://topnguyen.net)
- Support `[AllowAnonymous]` in `Action` and `Controller`.
- Support multiple attributes and multiple permissions with Permission Enums `[Authorize(Enums.Permission)]`.
- If multiple Authorize Attribute then conditional `AND` will be apply.
- If multiple Permission into an Attribute then conditional `OR` will be apply.
- Support `Action` combine authorization of `Controller` by `[CombineAuthorize]`.

### Hour to use

- Add Filter to `IServiceFilter` in `Startup`
```csharp
services.AddScoped<ApiModelValidationActionFilter>();
```
Use in `Base Controller`
```csharp
[ServiceFilter(typeof(ApiAuthorizeActionFilter))]
public class BaseController: Controller
...
```

- Then use in `Controller` or `Action`
```csharp
[Authorize(Enums.Permission.Test)]
public class TestApiController : BaseController
{
    ...
    // This action only allow user have (Test and Check) or (Test and AnotherCheck) permission
    [CombineAuthorize()]
    [Authorize(Enums.Permission.Check, Enums.Permission.AnotherCheck)]
    public IActionResult Test()
    {
        ...
    }
    ...
}
```