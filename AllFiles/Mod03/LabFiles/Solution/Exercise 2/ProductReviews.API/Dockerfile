FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
#ENV ASPNETCORE_DATABASE="Server=tcp:host.docker.internal,4444;Database=Mod1DB;User Id=sa;Password=Test_1234567;MultipleActiveResultSets=True;TrustServerCertificate=true"
#ENV ASPNETCORE_DATABASE="Server=mssql-ep;Database=Mod1DB;User Id=sa;Password=Test_1234567;MultipleActiveResultSets=True;TrustServerCertificate=true"
ENV ASPNETCORE_DATABASE="Server=tcp:host.docker.internal,4444;Database=Mod1DB;User Id=sa;Password=Test_1234567;MultipleActiveResultSets=True;TrustServerCertificate=true"

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY . .
WORKDIR /src/ProductReviews.API
RUN dotnet restore "ProductReviews.API.csproj"
RUN dotnet build "ProductReviews.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ProductReviews.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductReviews.API.dll"]
