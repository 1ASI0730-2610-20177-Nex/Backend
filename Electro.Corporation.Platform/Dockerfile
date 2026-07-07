FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Electro.Corporation.Platform/Electro.Corporation.Platform.csproj", "Electro.Corporation.Platform/"]
RUN dotnet restore "Electro.Corporation.Platform/Electro.Corporation.Platform.csproj"

COPY . .
WORKDIR "/src/Electro.Corporation.Platform"
RUN dotnet publish "Electro.Corporation.Platform.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Electro.Corporation.Platform.dll"]