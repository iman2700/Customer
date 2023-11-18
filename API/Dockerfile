
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy only the project file and restore dependencies to leverage caching
COPY ["API/API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"


# Copy the entire solution and build the application
COPY . .
WORKDIR "/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

 

# Use the  runtime image again for the final stage
FROM base AS final
WORKDIR /app

 

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "API.dll"]
