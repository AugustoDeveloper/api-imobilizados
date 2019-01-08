# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY /src/API/bin/Debug/netcoreapp2.1/publish/ .
ENTRYPOINT ["dotnet", "Imobilizados.API.dll"]