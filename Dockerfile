# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Signature/Signature.csproj", "Signature/"]
RUN dotnet restore "Signature/Signature.csproj"

COPY . .
WORKDIR "/src/Signature"
RUN dotnet publish "Signature.csproj" -c Release -o /app/publish

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Signature.dll"]
