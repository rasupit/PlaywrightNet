#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlaywrightNet/PlaywrightNet.csproj", "PlaywrightNet/"]
RUN dotnet restore "PlaywrightNet/PlaywrightNet.csproj"
COPY . .
WORKDIR "/src/PlaywrightNet"
RUN dotnet build "PlaywrightNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlaywrightNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN pwsh bin/Debug/net7.0/playwright.ps1 install

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlaywrightNet.dll"]