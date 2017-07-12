# Important Note
> Project Created by **Top Nguyen** (http://topnguyen.net)

## Install Auto Mapper
```markup
- Install-Package AutoMapper
- Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
```
- https://www.nuget.org/packages/AutoMapper/
- https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/

## Sample
```csharp
using AutoMapper;
using Puppy.AutoMapper;

public class ClassAToClassBProfile : Profile
{
    public ClassAToClassBProfile()
    {
        CreateMap<ClassA, ClassB>()
            .IgnoreAllNonExisting()
            .ForMember(dest => dest.PropertyOne, opt => opt.MapFrom(src => src.PropertyTwo))
            .BeforeMap((src, dest) =>
            {
               // Do Some Logic Before map data 
            })
            .AfterMap((src, dest) =>
            {
               // Do Some Logic After Finish map data
            });
    }
}
```