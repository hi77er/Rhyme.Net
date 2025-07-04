FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../Rhyme.Net.Infrastructure Rhyme.Net.Infrastructure/
COPY ../Rhyme.Net.UseCases Rhyme.Net.UseCases/
COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Rhyme.Net.CouponsForCampaignJob.dll"]