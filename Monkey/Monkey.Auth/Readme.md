﻿![Logo](favicon.ico)
# Monkey.Auth
> Project Created by [**Top Nguyen**](http://topnguyen.net)

# How to use
- Add Security options in appsettings.json
```json
"Authentication": {
    "SecretKey": "6c8992a0df9656e8cc4c1240a2545d3a"
}
```

- Add Services in Startup
```csharp
// [Authentication] Json Web Token
services.AddJwtAuth(ConfigurationRoot)
```

- Use Application Builder, please use Authentication before MVC
```csharp
// [Authentication] Json Web Token
app.UseJwtAuth()
```