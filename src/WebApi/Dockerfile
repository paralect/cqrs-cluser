FROM microsoft/aspnetcore

WORKDIR /app
COPY bin/Release/netcoreapp1.0/publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]
