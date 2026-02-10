FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Shared/Library.Shared/Library.Shared.csproj", "Shared/Library.Shared/"]
COPY ["src/Services/Party/Party.Domain/Party.Domain.csproj", "Services/Party/Party.Domain/"]
COPY ["src/Services/Party/Party.Application/Party.Application.csproj", "Services/Party/Party.Application/"]
COPY ["src/Services/Party/Party.Infrastructure/Party.Infrastructure.csproj", "Services/Party/Party.Infrastructure/"]
COPY ["src/Services/Party/Party.API/Party.API.csproj", "Services/Party/Party.API/"]
RUN dotnet restore "Services/Party/Party.API/Party.API.csproj"

COPY src/ .
WORKDIR "/src/Services/Party/Party.API"
RUN dotnet build "Party.API.csproj" -c Release -o /app/build
RUN dotnet publish "Party.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5001
ENTRYPOINT ["dotnet", "Party.API.dll"]
