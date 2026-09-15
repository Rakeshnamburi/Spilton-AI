FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY backend/Spilton.Api/Spilton.Api.csproj backend/Spilton.Api/
RUN dotnet restore backend/Spilton.Api/Spilton.Api.csproj
COPY backend/Spilton.Api backend/Spilton.Api
RUN dotnet publish backend/Spilton.Api/Spilton.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet","Spilton.Api.dll"]

