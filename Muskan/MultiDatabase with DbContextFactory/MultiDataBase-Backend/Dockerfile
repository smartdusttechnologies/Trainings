
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /multiDatabase

COPY MultiDataBase-Backend/MultiDatabase/MultiDatabase.csproj ./MultiDatabase/
COPY MultiDataBase-Backend/TestProject/TestProject.csproj ./TestProject/
RUN dotnet restore MultiDatabase/MultiDatabase.csproj

COPY MultiDataBase-Backend/MultiDatabase ./MultiDatabase/
COPY MultiDataBase-Backend/TestProject ./TestProject/
RUN dotnet publish MultiDatabase/MultiDatabase.csproj -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app


COPY --from=build /app .


EXPOSE 7122
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:7122


ENTRYPOINT ["dotnet", "MultiDatabase.dll"]
