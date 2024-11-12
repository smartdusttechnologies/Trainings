
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# C /multiDatabase

# COPY Controllers /multiDatabase/Controllers
# COPY Data /multiDatabase/Data
# COPY Migrations /multiDatabase/Migrations
# COPY Models /multiDatabase/Models
# COPY Properties /multiDatabase/Properties
# COPY Repository /multiDatabase/Repository
# COPY Views /multiDatabase/Views
# COPY wwwroot /multiDatabase/wwwroot
# COPY . .
# RUN dotnet restore

# # Publish the application
# RUN dotnet publish -c Release -o /app

# # Runtime stage
# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
# WORKDIR /app

# # Copy the build output to the runtime stage
# COPY --from=build /app .

# # Expose the port
# EXPOSE 7122

# # Set environment variables
# ENV ASPNETCORE_ENVIRONMENT=Development
# ENV ASPNETCORE_URLS=https://localhost:7122

# # Run the application
# ENTRYPOINT ["dotnet", "MultiDatabase.dll"]
