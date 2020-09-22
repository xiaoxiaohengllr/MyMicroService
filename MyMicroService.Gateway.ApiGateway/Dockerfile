#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Nuget.config", "MyMicroService.Gateway.ApiGateway/"]
COPY ["MyMicroService.Gateway.ApiGateway/MyMicroService.Gateway.ApiGateway.csproj", "MyMicroService.Gateway.ApiGateway/"]
RUN dotnet restore "MyMicroService.Gateway.ApiGateway/MyMicroService.Gateway.ApiGateway.csproj" --configfile "MyMicroService.Gateway.ApiGateway/Nuget.config"
COPY . .
WORKDIR "/src/MyMicroService.Gateway.ApiGateway"
RUN dotnet build "MyMicroService.Gateway.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyMicroService.Gateway.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyMicroService.Gateway.ApiGateway.dll"]