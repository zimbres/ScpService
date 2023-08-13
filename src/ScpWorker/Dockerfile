FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ScpWorker/ScpWorker.csproj", "src/ScpWorker/"]
RUN dotnet restore "src/ScpWorker/ScpWorker.csproj"
COPY . .
WORKDIR "/src/src/ScpWorker"
RUN dotnet build "ScpWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScpWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScpWorker.dll"]