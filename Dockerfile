# Définition de l'image de base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80
EXPOSE 443

# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copie du projet WebServiceM2Lib
COPY WebServicesM2.sln .
COPY WebServiceM2Lib/ WebServiceM2Lib/
COPY Messaging/ Messaging/
COPY . .


RUN dotnet restore Messaging/Messaging.fsproj


WORKDIR /src/Messaging
RUN dotnet build -c Release -o /app/build

# Étape finale
FROM base AS final
WORKDIR /app

# Copie des fichiers publiés
COPY --from=build /app/build .

# Point d'entrée
ENTRYPOINT ["dotnet", "Messaging.dll"]
