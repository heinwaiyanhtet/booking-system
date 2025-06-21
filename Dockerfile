FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["booking-system.csproj", "./"]
RUN dotnet restore "booking-system.csproj"
COPY . .
RUN dotnet publish "booking-system.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "booking-system.dll"]
