FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Agenda.sln ./
COPY src/Agenda.Api/Agenda.Api.csproj src/Agenda.Api/
RUN dotnet restore src/Agenda.Api/Agenda.Api.csproj

COPY src/Agenda.Api/ src/Agenda.Api/
WORKDIR /src/src/Agenda.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Agenda.Api.dll"]
