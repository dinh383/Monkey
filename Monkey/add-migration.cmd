pushd EatUp.Data.EF
dotnet ef migrations add %1 -v --context DbContext -o "Migrations"
dotnet ef database update -v --context DbContext
popd