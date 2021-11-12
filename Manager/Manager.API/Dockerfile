FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR "/src/Manager/Manager.API"
RUN dotnet publish "Manager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Manager.API.dll"]
