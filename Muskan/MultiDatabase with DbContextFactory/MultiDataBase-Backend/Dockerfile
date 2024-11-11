# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /multiDatabase

# Copy project files and restore dependencies
COPY MultiDatabase/MultiDatabase.csproj MultiDatabase/
COPY TestProject/TestProject.csproj TestProject/
RUN dotnet restore MultiDatabase/MultiDatabase.csproj

# Copy all source files and publish the app
COPY MultiDatabase/ MultiDatabase/
COPY TestProject/ TestProject/
RUN dotnet publish MultiDatabase/MultiDatabase.csproj -c Release -o /app

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app .

# Expose port and set environment variables
EXPOSE 7122
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:7122

# Run the application
ENTRYPOINT ["dotnet", "MultiDatabase.dll"]
