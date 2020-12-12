# FROM mcr.microsoft.com/dotnet/sdk:3.1
# WORKDIR /app
# EXPOSE 80

# COPY *.csproj ./
# RUN dotnet restore

# RUN dotnet build

# COPY . ./
# RUN dotnet publish -c Release -o out
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet out/NFCE.API.dll

# FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
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
# FROM mcr.microsoft.com/dotnet/aspnet:3.1
# WORKDIR /app
# COPY --from=build-env /app/out .
# ENTRYPOINT ["dotnet", "NFCE.API.dll"]

WORKDIR /app
EXPOSE 80

COPY ./out ./

CMD ASPNETCORE_URLS=http://*:$PORT dotnet out/NFCE.API.dll
