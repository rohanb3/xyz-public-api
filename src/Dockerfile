FROM mcr.microsoft.com/dotnet/core/sdk:2.2

COPY ./app /app

WORKDIR /app

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

EXPOSE 8083

ENTRYPOINT ["dotnet", "run", "-p", "/app/Xyzies.TWC.Public.Api/" ]

