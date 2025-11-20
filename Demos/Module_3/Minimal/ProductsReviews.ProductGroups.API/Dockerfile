# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Minimal/ProductsReviews.ProductGroups.API/ProductsReviews.ProductGroups.API.csproj", "Minimal/ProductsReviews.ProductGroups.API/"]
COPY ["ProductsReviews.DAL/ProductsReviews.DAL.EntityFramework.csproj", "ProductsReviews.DAL/"]
COPY ["ProductsReviews.DAL.Entities/ProductsReviews.DAL.Entities.csproj", "ProductsReviews.DAL.Entities/"]
COPY ["ProductsReviews.Interfaces/ProductsReviews.DAL.Interfaces.csproj", "ProductsReviews.Interfaces/"]
COPY ["ProductsReviews.Repositories.EntityFramework/ProductsReviews.DAL.Repositories.csproj", "ProductsReviews.Repositories.EntityFramework/"]
RUN dotnet restore "./Minimal/ProductsReviews.ProductGroups.API/ProductsReviews.ProductGroups.API.csproj"
COPY . .
WORKDIR "/src/Minimal/ProductsReviews.ProductGroups.API"
RUN dotnet build "./ProductsReviews.ProductGroups.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductsReviews.ProductGroups.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsReviews.ProductGroups.API.dll"]