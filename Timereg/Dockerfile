FROM mcr.microsoft.com/dotnet/core/sdk:3.1
COPY . /app
WORKDIR /app
RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS https://+:5001;http://+:5000
WORKDIR /app/out

ENTRYPOINT ["dotnet", "Timereg.dll"]
