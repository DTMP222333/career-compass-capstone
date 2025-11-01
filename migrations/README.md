# EF Core Migrations

dotnet tool update --global dotnet-ef

dotnet restore

dotnet ef migrations add InitialCreate --project src/infrastructure --startup-project src/api

dotnet ef database update --project src/infrastructure --startup-project src/api
