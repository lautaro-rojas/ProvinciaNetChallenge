# 1. Base image for execution
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Base image for compilation
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 3. Copy projects and restore packages
COPY ["src/ProvNetChallenge.WebApi/ProvNetChallenge.WebApi.csproj", "src/ProvNetChallenge.WebApi/"]
COPY ["src/ProvNetChallenge.Application/ProvNetChallenge.Application.csproj", "src/ProvNetChallenge.Application/"]
COPY ["src/ProvNetChallenge.Infrastructure/ProvNetChallenge.Infrastructure.csproj", "src/ProvNetChallenge.Infrastructure/"]
COPY ["src/ProvNetChallenge.Domain/ProvNetChallenge.Domain.csproj", "src/ProvNetChallenge.Domain/"]

# Restore dependencies from the main project (WebApi)
RUN dotnet restore "src/ProvNetChallenge.WebApi/ProvNetChallenge.WebApi.csproj"

# 4. Copy the rest of the code and build
COPY . .

# Compile
WORKDIR "/src/src/ProvNetChallenge.WebApi"
RUN dotnet build "ProvNetChallenge.WebApi.csproj" -c Release -o /app/build

# 5. Publish optimized version
FROM build AS publish
RUN dotnet publish "ProvNetChallenge.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 6. Final production image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProvNetChallenge.WebApi.dll"]