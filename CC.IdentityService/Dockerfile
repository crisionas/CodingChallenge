#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7082

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CC.IdentityService/CC.IdentityService.csproj", "CC.IdentityService/"]
COPY ["CC.Common/CC.Common.csproj", "CC.Common/"]
RUN dotnet restore "CC.IdentityService/CC.IdentityService.csproj"
COPY . .
WORKDIR "/src/CC.IdentityService"
RUN dotnet build "CC.IdentityService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CC.IdentityService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CC.IdentityService.dll"]
