FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TodoApi/TodoApi.csproj", "TodoApi/"]
RUN dotnet restore "TodoApi/TodoApi.csproj"
COPY . .
WORKDIR "/src/TodoApi"
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create healthcheck script
RUN echo '#!/bin/bash\n\
curl -f http://localhost/health || exit 1' > /app/healthcheck.sh \
    && chmod +x /app/healthcheck.sh

HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 CMD ["/app/healthcheck.sh"]

# Use environment variables for connection string
ENV ConnectionStrings__DefaultConnection="Server=db;Database=TodoDB;User Id=sa;Password=YourStrong@Passw0rd123;TrustServerCertificate=True"

ENTRYPOINT ["dotnet", "TodoApi.dll"]