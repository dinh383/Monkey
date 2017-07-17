![Logo](favicon.ico)
# Monkey.Data.EF
> Project Created by [**Top Nguyen**](http://topnguyen.net)
- This project is implementation of [`Monkey.Data`](../Monkey.Data/readme.md)
- Use [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/index)
  > EF Core is re-build, re-design version of Entity Framework. So, some function will not have and extra more feature in Core.

## Initial Database
Setup by Command Windows of current project 

```markup
<!-- Add migration via cmd (Initial is Name of the Migration) -->
dotnet ef migrations add Initial -v

<!-- Update/Sync code first to database via cmd -->
dotnet ef database update  -v
```

**Don't use/run Package Manager Console to do the above action**
**It will hang the Console and never stop without any result.**

# Special things in .csproj

```markup
  <PropertyGroup>
    <TargetFramework>netcoreapp1.1</TargetFramework>
    <ApplicationIcon>favicon.ico</ApplicationIcon>
    <Copyright>http://topnguyen.net</Copyright>
    
    <!-- Enable runtime config and runtime version, Need for entity framework DonetClioTool -->
    <GenerateRuntimeConfigurationFiles>true</GenerateRuntimeConfigurationFiles>
  </PropertyGroup>

  <!-- Entity Framework -->
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="1.1.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="1.1.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="1.1.1" />
    <!-- START Keep Runtime version is 1.0.0-* -->
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="1.1.0" />
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="1.0.0-*" />
    <!-- END -->
  </ItemGroup>
```