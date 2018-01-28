![Logo](favicon.ico)
# Monkey.Data.EF
> Project Created by [**Top Nguyen**](http://topnguyen.net)
- This project is implementation of [`Monkey.Data`](../Monkey.Data/readme.md)
- Use [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/index)
  > EF Core is re-build, re-design version of Entity Framework. So, some function will not have and extra more feature in Core.

- Don't query or save change async because EF have issue [5816](https://github.com/aspnet/EntityFrameworkCore/issues/5816)

- AspNetCore 2 already support for `TransactionScope` but EF Core not yet. Please view more detail at [Stack OverFlow](https://stackoverflow.com/questions/46577551/ef-core-2-0-transactionscope-error)

- Please view more detail at [Annoucing for AspNetCore 2](https://blogs.msdn.microsoft.com/dotnet/2017/05/12/announcing-ef-core-2-0-preview-1/)

## Initial Database
Setup by Command Windows of current project 

```xml
<!-- Add migration via cmd (Initial is Name of the Migration) -->
dotnet ef migrations add Initial -v

<!-- Update/Sync code first to database via cmd -->
dotnet ef database update  -v
```

**Don't use/run Package Manager Console to do the above action**
**It will hang the Console and never stop without any result.**

# Mapping
- Sample Entity Map
```csharp
public class UserMap : EntityTypeConfiguration<UserEntity>
{
    public override void Map(EntityTypeBuilder<UserEntity> builder)
    {
        base.Map(builder);
        builder.ToTable(nameof(UserEntity));
    }
}
```

# Special things in .csproj

```xml
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