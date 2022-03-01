#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["kurwa-attacks/kurwa-attacks.csproj", "kurwa-attacks/"]
RUN dotnet restore "kurwa-attacks/kurwa-attacks.csproj"
COPY . .
WORKDIR "/src/kurwa-attacks"
RUN dotnet build "kurwa-attacks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kurwa-attacks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kurwa-attacks.dll"]
