#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CameraSystem.Server/CameraSystem.Server.csproj", "CameraSystem.Server/"]
COPY ["CameraSystem.Shared/CameraSystem.Shared.csproj", "CameraSystem.Shared/"]
RUN dotnet restore "CameraSystem.Server/CameraSystem.Server.csproj"
COPY . .
WORKDIR "/src/CameraSystem.Server"
RUN dotnet build "CameraSystem.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CameraSystem.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CameraSystem.Server.dll"]