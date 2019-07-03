FROM microsoft/aspnetcore-build AS build
WORKDIR /code

COPY . ./
RUN dotnet restore EventApi.csproj


RUN dotnet publish --output /out/ --configuration Release

# build runtime image
FROM microsoft/aspnetcore:2.0

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "EventApi.dll"]