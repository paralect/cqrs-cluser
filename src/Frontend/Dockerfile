FROM microsoft/aspnetcore

WORKDIR /app
COPY bin/release/netcoreapp1.0/publish .

ENTRYPOINT ["dotnet", "Frontend.dll"]
