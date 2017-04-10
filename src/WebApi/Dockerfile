FROM microsoft/aspnetcore

WORKDIR /app
COPY bin/Debug/netcoreapp1.0/publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]
