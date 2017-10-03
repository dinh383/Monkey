![Logo](favicon.ico)
# Monkey.Core
> Project Created by [**Top Nguyen**](http://topnguyen.net)

- Interface Convention
  + Use postfix `Manager` for execuable interfaces.
  + Don't use any postfix for mark interfaces.

- SystemConfigs setup in appsettings.json, if not have specific setup, will use default setting.

```json
"ConnectionStrings": {
    "TOP": "Data Source=.;Initial Catalog=Monkey;User ID=Monkey;Password=Monkey;Trusted_Connection=False;MultipleActiveResultSets=True",
    "Staging": "Data Source=.;Initial Catalog=Monkey;User ID=Monkey;Password=Monkey;Trusted_Connection=False;MultipleActiveResultSets=True",
    "Production": "Data Source=.;Initial Catalog=Monkey;User ID=Monkey;Password=Monkey;Trusted_Connection=False;MultipleActiveResultSets=True"
},

"MvcPath": {
"WebRootFolderName": "wwwroot",
"AreasRootFolderName": "Areas",
"StaticsContents": [
        {
            // Portal Assets
            "Area": "Portal",
            "FolderRelativePath": "wwwroot", // relative path from area
            "HttpRequestPath": "/portal/assets", // use lower case
            "MaxAgeResponseHeader": "365.00:00:00"
        }
    ]
},

// [Auto Reload]
"PagedCollectionParameters": {
    "Skip": 0,
    "Take": 10,
    "MaxTake": 10000,
    "Terms": ""
}
```