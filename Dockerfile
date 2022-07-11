FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Weather.API/Weather.API.csproj", "Weather.API/"]
COPY ["Weather.Infrastructure/Weather.Infrastructure.csproj", "Weather.Infrastructure/"]
COPY ["Weather.BusinessLogic/Weather.BusinessLogic.csproj", "Weather.BusinessLogic/"]
RUN dotnet restore "Weather.API/Weather.API.csproj"
COPY . .
WORKDIR "/src/Weather.API"
RUN dotnet build "Weather.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Weather.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Weather.API.dll"]