FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Mundialito.Api/Mundialito.Api.csproj", "Mundialito.Api/"]
COPY ["Mundialito.Application/Mundialito.Application.csproj", "Mundialito.Application/"]
COPY ["Mundialito.Infrastructure/Mundialito.Infrastructure.csproj", "Mundialito.Infrastructure/"]
COPY ["Mundialito.Domain/Mundialito.Domain.csproj", "Mundialito.Domain/"]

RUN dotnet restore "Mundialito.Api/Mundialito.Api.csproj"

COPY . .

WORKDIR "/src/Mundialito.Api"
RUN dotnet publish "Mundialito.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mundialito.Api.dll"]