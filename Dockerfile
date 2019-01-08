# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY ./src/API/bin/Release/netcoreapp2.1/publish/ .
ENTRYPOINT ["dotnet", "Imobilizados.API.dll"]