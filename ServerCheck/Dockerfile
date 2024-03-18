FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tuwaiq-ServerCheck/Tuwaiq-ServerCheck.csproj", "Tuwaiq-ServerCheck/"]
RUN dotnet restore "Tuwaiq-ServerCheck/Tuwaiq-ServerCheck.csproj"
COPY . .
WORKDIR "/src/Tuwaiq-ServerCheck"
RUN dotnet build "Tuwaiq-ServerCheck.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tuwaiq-ServerCheck.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tuwaiq-ServerCheck.dll"]
