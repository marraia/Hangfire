FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Hangfire.API/Hangfire.API.csproj", "Hangfire.API/"]
RUN dotnet restore "Hangfire.API/Hangfire.API.csproj"
COPY . .
WORKDIR "/src/Hangfire.API"
RUN dotnet build "Hangfire.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Hangfire.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hangfire.API.dll"]