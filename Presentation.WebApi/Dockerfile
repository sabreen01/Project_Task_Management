FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["ProjectTaskManagement.sln", "./"]
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["Core.Application/Core.Application.csproj", "Core.Application/"]
COPY ["Infrastructure.Persistence/Infrastructure.Persistence.csproj", "Infrastructure.Persistence/"]
COPY ["Presentation.WebApi/Presentation.WebApi.csproj", "Presentation.WebApi/"]
COPY ["Tests/Tests.csproj", "Tests/"]

# Restore dependencies
RUN dotnet restore

# Copy the remaining files
COPY . .

# Build and publish
WORKDIR "/src/Presentation.WebApi"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Presentation.WebApi.dll"]
