# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app ./

# Railway provides the PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

EXPOSE ${PORT}

# Start the application
ENTRYPOINT ["dotnet", "ImposterGame.dll"]
```

4. **Save the file**

---

### Step 2: Add .dockerignore File (Optional but Recommended)

1. **Create a file** called `.dockerignore` in the ROOT of your project

2. **Paste this:**
```
**/.git
**/.gitignore
**/.vs
**/.vscode
**/*.*proj.user
**/bin
**/obj
**/out
**/.dockerignore
**/Dockerfile
**/*.md
**/.github