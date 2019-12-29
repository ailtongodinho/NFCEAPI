FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./publish .

ENTRYPOINT ["dotnet", "NFCE.API.dll"]