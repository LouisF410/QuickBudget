#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuickBudget.API/QuickBudget.API.csproj", "QuickBudget.API/"]
RUN dotnet restore "QuickBudget.API/QuickBudget.API.csproj"
COPY . .
WORKDIR "/src/QuickBudget.API"
RUN dotnet build "QuickBudget.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickBudget.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickBudget.API.dll"]