FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Shared/Library.Shared/Library.Shared.csproj", "Shared/Library.Shared/"]
COPY ["src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj", "Services/Catalog/Catalog.Domain/"]
COPY ["src/Services/Catalog/Catalog.Application/Catalog.Application.csproj", "Services/Catalog/Catalog.Application/"]
COPY ["src/Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Services/Catalog/Catalog.Infrastructure/"]
COPY ["src/Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"

COPY src/ .
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5002
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
