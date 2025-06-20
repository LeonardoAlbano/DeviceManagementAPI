FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DeviceManagement.Api/DeviceManagement.Api.csproj", "src/DeviceManagement.Api/"]
COPY ["src/DeviceManagement.Application/DeviceManagement.Application.csproj", "src/DeviceManagement.Application/"]
COPY ["src/DeviceManagement.Domain/DeviceManagement.Domain.csproj", "src/DeviceManagement.Domain/"]
COPY ["src/DeviceManagement.Infrastructure/DeviceManagement.Infrastructure.csproj", "src/DeviceManagement.Infrastructure/"]
COPY ["src/DeviceManagement.Communication/DeviceManagement.Communication.csproj", "src/DeviceManagement.Communication/"]
COPY ["src/DeviceManagement.Exception/DeviceManagement.Exception.csproj", "src/DeviceManagement.Exception/"]

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
