#FROM microsoft/aspnetcore:2.0
#ARG source
#WORKDIR /app
#EXPOSE 80
#EXPOSE 3306
#COPY ${source:-obj/Docker/publish} .
#ENTRYPOINT ["dotnet", "ujetsapi.dll"]

 # Sample contents of Dockerfile
 # Stage 1
 FROM microsoft/aspnetcore-build AS builder
 WORKDIR /source

 # caches restore result by copying csproj file separately
 COPY *.csproj .
 RUN dotnet restore

 # copies the rest of your code
 COPY . .
 RUN dotnet publish --output /app/ --configuration Release

 # Stage 2
 FROM microsoft/aspnetcore
 WORKDIR /app
 COPY --from=builder /app .
 ENTRYPOINT ["dotnet", "ujetsapi.dll"]
