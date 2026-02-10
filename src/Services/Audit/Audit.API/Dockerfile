FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Shared/Library.Shared/Library.Shared.csproj", "Shared/Library.Shared/"]
COPY ["src/Services/Audit/Audit.Domain/Audit.Domain.csproj", "Services/Audit/Audit.Domain/"]
COPY ["src/Services/Audit/Audit.Application/Audit.Application.csproj", "Services/Audit/Audit.Application/"]
COPY ["src/Services/Audit/Audit.Infrastructure/Audit.Infrastructure.csproj", "Services/Audit/Audit.Infrastructure/"]
COPY ["src/Services/Audit/Audit.API/Audit.API.csproj", "Services/Audit/Audit.API/"]
RUN dotnet restore "Services/Audit/Audit.API/Audit.API.csproj"

COPY src/ .
WORKDIR "/src/Services/Audit/Audit.API"
RUN dotnet build "Audit.API.csproj" -c Release -o /app/build
RUN dotnet publish "Audit.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5004
ENTRYPOINT ["dotnet", "Audit.API.dll"]
