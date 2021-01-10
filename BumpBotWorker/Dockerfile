FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["BumpBotWorker/BumpBotWorker.csproj", "BumpBotWorker/"]
RUN dotnet restore "BumpBotWorker/BumpBotWorker.csproj"
COPY . .
WORKDIR "/src/BumpBotWorker"
RUN dotnet build "BumpBotWorker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BumpBotWorker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BumpBotWorker.dll"]