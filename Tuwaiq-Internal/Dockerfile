FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TuwaiqRecruitment/TuwaiqRecruitment.csproj", "TuwaiqRecruitment/"]

RUN dotnet nuget add source https://nuget.tuwaiqdev.com/v3/index.json -n BaGet

RUN dotnet restore "TuwaiqRecruitment/TuwaiqRecruitment.csproj" 
COPY . .
WORKDIR "/src/TuwaiqRecruitment"
RUN dotnet build "TuwaiqRecruitment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TuwaiqRecruitment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TuwaiqRecruitment.dll"]
