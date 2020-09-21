#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Nuget.config", "MyMicroService.Service.TimingSchedulingService/"]
COPY ["MyMicroService.Service.TimingSchedulingService/MyMicroService.Service.TimingSchedulingService.csproj", "MyMicroService.Service.TimingSchedulingService/"]
RUN dotnet restore "MyMicroService.Service.TimingSchedulingService/MyMicroService.Service.TimingSchedulingService.csproj" --configfile "MyMicroService.Service.TimingSchedulingService/Nuget.config"
COPY . .
WORKDIR "/src/MyMicroService.Service.TimingSchedulingService"
RUN dotnet build "MyMicroService.Service.TimingSchedulingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyMicroService.Service.TimingSchedulingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyMicroService.Service.TimingSchedulingService.dll"]