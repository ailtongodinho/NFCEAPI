FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
EXPOSE 80

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet out/NFCE.API.dll
# Build da imagem
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NFCE.API.dll"]