FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build
WORKDIR /app

COPY ./MqttService ./MqttService
COPY ./ServiceBus.Contracts ./ServiceBus.Contracts

RUN dotnet restore ./ServiceBus.Contracts/ServiceBus.Contracts.csproj
RUN dotnet restore ./MqttService/MqttService.csproj

RUN dotnet publish -c Release -o out ./MqttService/MqttService.csproj

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MqttService.dll"]
