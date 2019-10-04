#!/bin/bash

# Raindream Build-And-Run Script

cd src
dotnet build Raindream.csproj
dotnet run --project Raindream.csproj