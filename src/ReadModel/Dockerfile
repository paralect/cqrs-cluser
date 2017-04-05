FROM microsoft/dotnet

WORKDIR /app
COPY bin/Debug/netcoreapp1.0/publish .

ENTRYPOINT ["dotnet", "ReadModel.dll"]