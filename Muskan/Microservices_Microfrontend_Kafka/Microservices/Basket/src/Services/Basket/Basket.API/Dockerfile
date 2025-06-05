# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

 
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Basket/Basket.API/Basket.API.csproj", "Services/Basket/Basket.API/"]
COPY ["BuildingBlock/BuildingBlock.Messaging/BuildingBlock.Messaging.csproj", "BuildingBlock/BuildingBlock.Messaging/"]
COPY ["BuildingBlock/BuildingBlock/BuildingBlock.csproj", "BuildingBlock/BuildingBlock/"]
RUN dotnet restore "./Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Basket.API"
RUN dotnet build "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root

RUN apt-get update && apt-get install -y ca-certificates openssl && update-ca-certificates

RUN mkdir -p /app/Certs
COPY Services/Basket/Basket.API/Certs/devcert.pfx /app/Certs/

# Extract public cert and add to trusted CA store
RUN openssl pkcs12 -in /app/Certs/devcert.pfx -nokeys -out /usr/local/share/ca-certificates/devcert.crt -passin pass:1234 && \
    update-ca-certificates

RUN chmod 644 /usr/local/share/ca-certificates/devcert.crt

USER $APP_UID
ENTRYPOINT ["dotnet", "Basket.API.dll"]