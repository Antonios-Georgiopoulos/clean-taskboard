# ---------- Stage 1: Build & publish the application ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY CleanTaskBoard.slnx ./
COPY src/CleanTaskBoard.Api/CleanTaskBoard.Api.csproj src/CleanTaskBoard.Api/
COPY src/CleanTaskBoard.Application/CleanTaskBoard.Application.csproj src/CleanTaskBoard.Application/
COPY src/CleanTaskBoard.Domain/CleanTaskBoard.Domain.csproj src/CleanTaskBoard.Domain/
COPY src/CleanTaskBoard.Infrastructure/CleanTaskBoard.Infrastructure.csproj src/CleanTaskBoard.Infrastructure/

# Restore dependencies
RUN dotnet restore CleanTaskBoard.slnx

# Copy the rest of the source code
COPY . .

# Publish the API project in Release mode
RUN dotnet publish src/CleanTaskBoard.Api/CleanTaskBoard.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ---------- Stage 2: Runtime image ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Configure ASP.NET Core to listen on port 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080

# Expose container port
EXPOSE 8080

# Entrypoint to run the API
ENTRYPOINT ["dotnet", "CleanTaskBoard.Api.dll"]
