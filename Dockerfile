FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app
# EXPOSE 8080
EXPOSE 8080
# copy .csproj & restore as distinct layers
COPY "LuxonT.sln" "LuxonT.sln"
COPY "API/API.csproj" "API/API.csproj"
COPY "Application/Application.csproj" "Application/Application.csproj"
COPY "Domain/Domain.csproj" "Domain/Domain.csproj"
COPY "Infrastructure/Infrastructure.csproj" "Infrastructure/Infrastructure.csproj"
COPY "Persistence/Persistence.csproj" "Persistence/Persistence.csproj"

RUN dotnet restore "LuxonT.sln"

# Copy else & build
COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

#  build runtime img
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out . 
ENTRYPOINT ["dotnet", "API.dll"]