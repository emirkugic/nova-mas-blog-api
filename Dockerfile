# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set the SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["nova-mas-blog-api.csproj", "./"]
RUN dotnet restore "nova-mas-blog-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "nova-mas-blog-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "nova-mas-blog-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nova-mas-blog-api.dll"]
