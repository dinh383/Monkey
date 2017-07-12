# Important Note
> Project Created by **Top Nguyen** (http://topnguyen.net)
- Just add dependencies and use Framework, Core and Service (Contract only)
- Don't add all other project dependencies (reference)
- Follow Readme.md of Puppy.DependencyInjection

# Deploy Note
1. Create Publish Profile to Server through Visual Studio (Remember Create Server User to use in create profile wizard)
   - Manual right click of folder and click publish for make sure all file is published
   - launchSettings.json
   - Assets
   - template
   - api-doc.xml file
   - Files for SEO
2. Go to server Set Application Pool .NET CLR Version to No Managed Code
3. Advance Setting: Always Running, Load User Profile: true.
4. Go to deployed folder check and set full permission for users you use such as Default IIS user, Application Pool User.
5. Go to deployed folderrun CMD: dotnet <project dll/exe name>

# Db Note:
Remember to config DbContext if use Entity Framework in Startup, without it when run cmd in publish folder will throw null error
```csharp
// Use Entity Framework
services.AddDbContext<Data.EF.DbContext>(builder =>
builder.UseSqlServer(
    ConfigureHelper.Configuration.GetConnectionString(ConfigureHelper.Environment.EnvironmentName),
    options => options.MigrationsAssembly(typeof(IDataModule).GetTypeInfo().Assembly.GetName().Name)));
```

# Restful Note:
```csharp
return NotFound/Ok/Created/BadRequest and so on Don't put simple type like string or int ...
Need create object/dynamic to response for aspcore can cast to json or xml (because http request header is json or xml)
If not follow this rule, exception occur then return no content + header error
```

# Hangfire Note:
```csharp
Let class PerResolveDependency if they have own method not in Interfaces:
	BackgroundJob.Enqueue(() => Method));	 // make sure Method is public and allow access from external assembly.
Let use Job with Interface by DI:
	BackgroundJob.Enqueue<Interface>(x => x.Method));
```