FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Shared/Library.Shared/Library.Shared.csproj", "Shared/Library.Shared/"]
COPY ["src/Services/Reservation/Reservation.Domain/Reservation.Domain.csproj", "Services/Reservation/Reservation.Domain/"]
COPY ["src/Services/Reservation/Reservation.Application/Reservation.Application.csproj", "Services/Reservation/Reservation.Application/"]
COPY ["src/Services/Reservation/Reservation.Infrastructure/Reservation.Infrastructure.csproj", "Services/Reservation/Reservation.Infrastructure/"]
COPY ["src/Services/Reservation/Reservation.API/Reservation.API.csproj", "Services/Reservation/Reservation.API/"]
RUN dotnet restore "Services/Reservation/Reservation.API/Reservation.API.csproj"

COPY src/ .
WORKDIR "/src/Services/Reservation/Reservation.API"
RUN dotnet build "Reservation.API.csproj" -c Release -o /app/build
RUN dotnet publish "Reservation.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5003
ENTRYPOINT ["dotnet", "Reservation.API.dll"]
