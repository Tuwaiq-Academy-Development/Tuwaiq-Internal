FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tuwaiq-Internal/Tuwaiq-Internal.csproj", "Tuwaiq-Internal/"]

RUN dotnet nuget add source https://nuget.tuwaiqdev.com/v3/index.json -n BaGet

RUN dotnet restore "Tuwaiq-Internal/Tuwaiq-Internal.csproj"
COPY . .
WORKDIR "/src/Tuwaiq-Internal"
RUN dotnet build "Tuwaiq-Internal.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tuwaiq-Internal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tuwaiq-Internal.dll"]



