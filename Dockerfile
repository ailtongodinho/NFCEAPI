# FROM mcr.microsoft.com/dotnet/sdk:5.0
# WORKDIR /app
# EXPOSE 80

# COPY *.csproj ./
# RUN dotnet restore

# RUN dotnet build

# COPY . ./
# RUN dotnet publish -c Release -o out
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet out/NFCE.API.dll

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
# WORKDIR /app

# #Exibir a versão do .NET
# RUN dotnet --version

# # Copiar csproj e restaurar dependencias
# COPY *.csproj ./
# RUN dotnet restore

# # Build da aplicacao
# COPY . ./
# RUN dotnet publish -c Release -o out

# # Build da imagem
# FROM mcr.microsoft.com/dotnet/aspnet:5.0
# WORKDIR /app
# COPY --from=build-env /app/out .
# ENTRYPOINT ["dotnet", "NFCE.API.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore 
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
# Padrão de container ASP.NET
# ENTRYPOINT ["dotnet", "CarterAPI.dll"]
# Opção utilizada pelo Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet NFCE.API.dll