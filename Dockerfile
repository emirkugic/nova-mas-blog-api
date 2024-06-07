# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildgit
# WORKDIR /src

# COPY ["nova-mas-blog-api.csproj", "./"]
# RUN dotnet restore "nova-mas-blog-api.csproj"

# COPY . .

# RUN dotnet build "nova-mas-blog-api.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "nova-mas-blog-api.csproj" -c Release -o /app/publish

# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "nova-mas-blog-api.dll"]