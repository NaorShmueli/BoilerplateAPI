From mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Logic/Logic.csproj", "Logic/"]
COPY ["Authentications/Authentications.csproj", "Authentications/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["BoilerplateAPI/BoilerplateAPI.csproj", "BoilerplateAPI/"]

RUN dotnet restore "BoilerplateAPI/BoilerplateAPI.csproj"
COPY . .
WORKDIR "/src/BoilerplateAPI"

RUN dotnet build "BoilerplateAPI.csproj" -c Release -o /app/build

FROM build as Publish
RUN dotnet publish "BoilerplateAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BoilerplateAPI.dll"]

