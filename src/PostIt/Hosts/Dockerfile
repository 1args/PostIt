FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PostIt/Hosts/Hosts.csproj", "PostIt/Hosts/"]
COPY ["PostIt/Contracts/Contracts.csproj", "PostIt/Contracts/"]
COPY ["PostIt/Infrastructure/Infrastructure.csproj", "PostIt/Infrastructure/"]
COPY ["PostIt/Application/Application.csproj", "PostIt/Application/"]
COPY ["PostIt/Domain/Domain.csproj", "PostIt/Domain/"]
COPY ["PostIt/Tests/Tests.csproj", "PostIt/Tests/"]
COPY ["PostIt/Common/Common.csproj", "PostIt/Common/"]
COPY ["PostIt/Migrations/Migrations.csproj", "PostIt/Migrations/"]
RUN dotnet restore "./PostIt/Hosts/Hosts.csproj"
COPY . .
WORKDIR "/src/UrlShortener/Hosts"
RUN dotnet build "./Hosts.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Hosts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hosts.dll"]