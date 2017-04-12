FROM microsoft/dotnet

WORKDIR /app
COPY bin/release/netcoreapp1.0/publish .

ENTRYPOINT ["dotnet", "ReadModel.dll"]
