# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution file
COPY TestCase-TetaCode.slnx ./

# Copy project files
COPY src/TestCase.Domain/TestCase.Domain.csproj ./src/TestCase.Domain/
COPY src/TestCase.Application/TestCase.Application.csproj ./src/TestCase.Application/
COPY src/TestCase.Infrastructure/TestCase.Infrastructure.csproj ./src/TestCase.Infrastructure/
COPY src/TestCase.WebAPI/TestCase.WebAPI.csproj ./src/TestCase.WebAPI/

# Restore dependencies
RUN dotnet restore src/TestCase.WebAPI/TestCase.WebAPI.csproj

# Copy the entire source code
COPY src/ ./src/

# Build the application
RUN dotnet build src/TestCase.WebAPI/TestCase.WebAPI.csproj -c Release --no-restore

# Publish the WebAPI project
RUN dotnet publish src/TestCase.WebAPI/TestCase.WebAPI.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Create wwwroot directories
RUN mkdir -p wwwroot/uploads/grades

# Expose the port
EXPOSE 5000

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Define the entry point
ENTRYPOINT ["dotnet", "TestCase.WebAPI.dll"]