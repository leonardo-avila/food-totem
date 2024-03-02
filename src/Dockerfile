#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Adapters/Driver/FoodTotem.API/FoodTotem.API.csproj", "Adapters/Driver/FoodTotem.API/"]
COPY ["Core/FoodTotem.Domain.Core/FoodTotem.Domain.Core.csproj", "Core/FoodTotem.Domain.Core/"]
COPY ["Core/FoodTotem.Identity.UseCase/FoodTotem.Identity.UseCase.csproj", "Core/FoodTotem.Identity.UseCase/"]
COPY ["Core/FoodTotem.Identity.Domain/FoodTotem.Identity.Domain.csproj", "Core/FoodTotem.Identity.Domain/"]
COPY ["Core/FoodTotem.Data.Core/FoodTotem.Data.Core.csproj", "Core/FoodTotem.Data.Core/"]
COPY ["Adapters/Driven/FoodTotem.Gateways.MySQL/FoodTotem.Gateways.MySQL.csproj", "Adapters/Driven/FoodTotem.Gateways.MySQL/"]
COPY ["Adapters/Driven/FoodTotem.Gateways.Http/FoodTotem.Gateways.Http.csproj", "Adapters/Driven/FoodTotem.Gateways.Http/"]
RUN dotnet restore "Adapters/Driver/FoodTotem.API/FoodTotem.API.csproj"
COPY . .
WORKDIR "/src/Adapters/Driver/FoodTotem.API"
RUN dotnet build "FoodTotem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodTotem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodTotem.API.dll"]