FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV TZ=America/Sao_Paulo

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Portfolio.Api/Portfolio.Api.csproj", "Portfolio.Api/"]
COPY ["Portfolio.Crosscutting/Portfolio.Crosscutting.csproj", "Portfolio.Crosscutting/"]
COPY ["Portfolio.Infrastructure/Portfolio.Infrastructure.csproj", "Portfolio.Infrastructure/"]
COPY ["Portfolio.Application/Portfolio.Application.csproj", "Portfolio.Application/"]
COPY ["Portfolio.Domain/Portfolio.Domain.csproj", "Portfolio.Domain/"]
COPY ["Portfolio.Data/Portfolio.Data.csproj", "Portfolio.Data/"]
RUN dotnet restore "Portfolio.Api/Portfolio.Api.csproj"
COPY . .
WORKDIR "/src/Portfolio.Api"
RUN dotnet build "Portfolio.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Portfolio.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]