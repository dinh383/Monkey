# Important Note
>  Created by **Top Nguyen** (http://topnguyen.net)

- use appsettings.json to config default cache duration
```json
  "Redis": {
    "ConnectionString": "localhost:<redis service port>",
    "InstanceName": "Monkey"
  }

```
- In Startup.cs
```csharp
	string redisConnection = Configuration["Redis:ConnectionString"];
	string redisInstance = Configuration["Redis:InstanceName"];

	services.AddSingleton<IDistributedCache>(factory =>
	{
		var cache = new RedisCache(new RedisCacheOptions
		{
			Configuration = redisConnection,
			InstanceName = redisInstance
		});

		return cache;
	});
```