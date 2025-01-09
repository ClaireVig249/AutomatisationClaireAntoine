# Étape de base avec runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copier tous les fichiers sources
COPY ./ ./

# Restaurer les dépendances
RUN dotnet restore "IntDB.csproj"

# Installer le package Minio
RUN dotnet add package Minio

# Construire l'application
RUN dotnet build "IntDB.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IntDB.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IntDB.dll"]