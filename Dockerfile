FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CorporateCopilot.sln ./
COPY src/CorporateCopilot.Api/CorporateCopilot.Api.csproj src/CorporateCopilot.Api/
RUN dotnet restore src/CorporateCopilot.Api/CorporateCopilot.Api.csproj

COPY src/CorporateCopilot.Api/ src/CorporateCopilot.Api/
RUN dotnet publish src/CorporateCopilot.Api/CorporateCopilot.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CorporateCopilot.Api.dll"]
