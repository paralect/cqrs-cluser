#!/bin/bash

dotnet publish "./Frontend" -c release
dotnet publish "./WebApi" -c release 
dotnet publish "./WriteModel" -c release
dotnet publish "./ReadModel" -c release