FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./PocketWallet/PocketWallet.csproj", "PocketWallet/"]
RUN dotnet restore "PocketWallet/PocketWallet.csproj"
COPY . .
WORKDIR "/src/PocketWallet"
RUN dotnet build "PocketWallet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PocketWallet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PocketWallet.dll"]
