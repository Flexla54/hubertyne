FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build
WORKDIR /app

COPY ./ProvisionService ./
COPY ./ServiceBus.Contracts ./

RUN dotnet restore ./ServiceBus.Contracts.csproj
RUN dotnet restore ./ProvisionService.csproj

RUN dotnet publish -c Release -o out ./ProvisionService.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ProvisionService.dll"]
