# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining application files
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the official .NET runtime image for execution
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

#expose the port your application will run on
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "UserManagementAPI.dll"]
