FROM microsoft/dotnet:2.1-sdk as build-env
WORKDIR /app

COPY . ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/src/API/out .
ENTRYPOINT ["dotnet", "Imobilizados.API.dll"]