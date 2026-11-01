FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY Directory.Build.props Directory.Packages.props ./
COPY src/Shopfront.Domain/Shopfront.Domain.csproj src/Shopfront.Domain/
COPY src/Shopfront.Application/Shopfront.Application.csproj src/Shopfront.Application/
COPY src/Shopfront.Infrastructure/Shopfront.Infrastructure.csproj src/Shopfront.Infrastructure/
COPY src/Shopfront.WebApi/Shopfront.WebApi.csproj src/Shopfront.WebApi/
RUN dotnet restore src/Shopfront.WebApi/Shopfront.WebApi.csproj

COPY src/ src/
RUN dotnet publish src/Shopfront.WebApi/Shopfront.WebApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Shopfront.WebApi.dll"]
