# Giai đoạn 1: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Giai đoạn 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["tranquoctuu_2123110477.csproj", "./"]
RUN dotnet restore "tranquoctuu_2123110477.csproj"

COPY . .
RUN dotnet build "tranquoctuu_2123110477.csproj" -c Release -o /app/build

# Giai đoạn 3: Publish
FROM build AS publish
RUN dotnet publish "tranquoctuu_2123110477.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn cuối: Run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tranquoctuu_2123110477.dll"]