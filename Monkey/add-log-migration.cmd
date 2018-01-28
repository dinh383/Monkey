pushd EatUp.Data.EF
dotnet ef migrations add %1 -v --context LogDbContext -o "LogMigrations"
dotnet ef database update -v --context LogDbContext
popd