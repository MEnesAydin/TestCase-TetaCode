# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy solution file
COPY PriceSyncPro.sln ./

# Copy project files
COPY src/PriceSyncPro.Domain/PriceSyncPro.Domain.csproj ./src/PriceSyncPro.Domain/
COPY src/PriceSyncPro.Application/PriceSyncPro.Application.csproj ./src/PriceSyncPro.Application/
COPY src/PriceSyncPro.Infrastructure/PriceSyncPro.Infrastructure.csproj ./src/PriceSyncPro.Infrastructure/
COPY src/PriceSyncPro.ServiceDefaults/PriceSyncPro.ServiceDefaults.csproj ./src/PriceSyncPro.ServiceDefaults/
COPY src/PriceSyncPro.WebAPI/PriceSyncPro.WebAPI.csproj ./src/PriceSyncPro.WebAPI/
COPY src/PriceSyncPro.AppHost/PriceSyncPro.AppHost.csproj ./src/PriceSyncPro.AppHost/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the WebAPI project
RUN dotnet publish src/PriceSyncPro.WebAPI/PriceSyncPro.WebAPI.csproj -c Release -o /app/publish --no-restore

# Use the official .NET 9.0 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 5000

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Define the entry point
ENTRYPOINT ["dotnet", "PriceSyncPro.WebAPI.dll"]
