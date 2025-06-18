FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DeviceManagement.Api/DeviceManagement.Api.csproj", "src/DeviceManagement.Api/"]
RUN dotnet restore "src/DeviceManagement.Api/DeviceManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/DeviceManagement.Api"
RUN dotnet build "./DeviceManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeviceManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceManagement.Api.dll"]
