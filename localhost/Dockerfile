FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PostIt.Api/PostIt.Api.csproj", "src/PostIt.Api/"]
COPY ["src/PostIt.Application/PostIt.Application.csproj", "src/PostIt.Application/"]
COPY ["src/PostIt.Contracts/PostIt.Contracts.csproj", "src/PostIt.Contracts/"]
COPY ["src/PostIt.Domain/PostIt.Domain.csproj", "src/PostIt.Domain/"]
COPY ["src/PostIt.Infrastructure/PostIt.Infrastructure.csproj", "src/PostIt.Infrastructure/"]
RUN dotnet restore "src/PostIt.Api/PostIt.Api.csproj"
COPY . .
WORKDIR "/src/PostIt.Api"
RUN dotnet build "PostIt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PostIt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostIt.Api.dll"]
