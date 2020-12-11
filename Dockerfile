FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /app
EXPOSE 80

COPY *.csproj ./
RUN dotnet restore

RUN dotnet build

COPY . ./
RUN dotnet publish -c Release -o out
CMD ASPNETCORE_URLS=http://*:$PORT dotnet out/NFCE.API.dll