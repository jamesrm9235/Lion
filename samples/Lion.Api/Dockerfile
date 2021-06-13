FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
COPY ["/bin/Release/netcoreapp3.1/publish", "app/publish/"]
WORKDIR /app/publish
ENTRYPOINT ["dotnet", "Lion.Api.dll"]
