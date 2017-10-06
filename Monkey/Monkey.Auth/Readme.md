![Logo](favicon.ico)
# Monkey.Auth
> Project Created by [**Top Nguyen**](http://topnguyen.net)

- Support Authentication via JWT Token and Cookie.
- Secure Cookie by Encrypt and Decrypt.
- Secure JWT by Signed Secret Key.
- Secure Password with Password Hash best practice.
- Support Authorization by `AuthAttribute` Attribute with Filter `ApiAuthActionFilter`. See [more information](Filters/Readme.md).

# How to use
- Add Security options in appsettings.json

```javascript
 // [Auto Reload]
"Authentication": {
    "SecretKey": "6c8992a0df9656e8cc4c1240a2545d3a", // Update for security purpose
    "AccessTokenExpireIn": "00:30:00"
}
```

- Add Services in Startup

```csharp
// [Authentication]Json Web Token + Cookie 
services.AddHybridAuth(ConfigurationRoot)
```

- Use Application Builder, please use Authentication before MVC
```csharp
// [Authentication] Json Web Token + Cookie
app.UseHybridAuth()
```