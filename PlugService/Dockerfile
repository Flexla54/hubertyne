FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build
WORKDIR /app

COPY ./PlugService ./
COPY ./ServiceBus.Contracts ./

RUN dotnet restore ./ServiceBus.Contracts.csproj
RUN dotnet restore ./PlugService.csproj

RUN dotnet publish -c Release -o out ./PlugService.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "PlugService.dll"]
