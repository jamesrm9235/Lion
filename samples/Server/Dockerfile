FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["samples/Server/Server.csproj", "samples/Server/"]
COPY ["src/Lion.Storage.SqlServer/Lion.Storage.SqlServer.csproj", "src/Lion.Storage.SqlServer/"]
COPY ["src/Lion.Common/Lion.Common.csproj", "src/Lion.Common/"]
COPY ["src/Lion.Server/Lion.Server.csproj", "src/Lion.Server/"]
RUN dotnet restore "samples/Server/Server.csproj"
COPY . .
WORKDIR "/src/samples/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Server.dll"]
