# FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
# WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
# RUN dotnet restore Rhyme.Net.sln
RUN dotnet publish Rhyme.Net.CouponsForCampaignJob -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Rhyme.Net.CouponsForCampaignJob.dll"]