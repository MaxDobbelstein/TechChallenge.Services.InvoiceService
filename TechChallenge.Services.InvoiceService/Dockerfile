FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.19-amd64 AS base
ENV COMPlus_EnableDiagnostics=0
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LANG de_DE.UTF-8
ENV LANGUAGE de_DE.UTF-8
ENV LC_ALL de_DE.UTF-8
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0.203-alpine3.19-amd64 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallenge.Services.InvoiceService/TechChallenge.Services.InvoiceService.csproj", "TechChallenge.Services.InvoiceService/"]
RUN dotnet restore "./TechChallenge.Services.InvoiceService/TechChallenge.Services.InvoiceService.csproj"
COPY . .
WORKDIR "/src/TechChallenge.Services.InvoiceService"
RUN dotnet build "./TechChallenge.Services.InvoiceService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge.Services.InvoiceService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge.Services.InvoiceService.dll"]