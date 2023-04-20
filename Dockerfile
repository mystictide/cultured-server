FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cultured-server/cultured.server.csproj", "cultured-server/"]
RUN dotnet restore "cultured-server/cultured.server.csproj"
COPY . .
WORKDIR "/src/cultured-server"
RUN dotnet build "cultured.server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cultured.server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=https://+:7474
ENV ASPNETCORE_HTTP_PORT=https://+:7474
EXPOSE 7474
ENTRYPOINT ["dotnet", "cultured.server.dll", "--urls", "https://+:7474"]