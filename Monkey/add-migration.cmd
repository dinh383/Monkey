pushd Monkey.Data.EF
dotnet ef migrations add %1 -v
dotnet ef database update -v
popd