# Use official .NET SDK as the environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY . ./
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy built application 
COPY --from=build /out ./

# Expose the ports (standardised ports)
EXPOSE 80
EXPOSE 443

# Set entrypoint to run the app
ENTRYPOINT ["dotnet", "BookReviewApi.dll"]
