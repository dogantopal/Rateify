FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

COPY Rateify.sln Rateify.sln
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY src/RatingService/RatingService.csproj src/RatingService/RatingService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY tests/NotificationService.IntegrationTests/NotificationService.IntegrationTests.csproj tests/NotificationService.IntegrationTests/NotificationService.IntegrationTests.csproj
COPY tests/RatingService.IntegrationTests/RatingService.IntegrationTests.csproj tests/RatingService.IntegrationTests/RatingService.IntegrationTests.csproj

RUN dotnet restore Rateify.sln

COPY src/NotificationService src/NotificationService
COPY src/Contracts src/Contracts

WORKDIR /app/src/NotificationService
RUN dotnet publish -c Release -o /app/src/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "NotificationService.dll" ]