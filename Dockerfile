# Etapa 1: Build de la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Instalar herramientas EF Core globalmente
RUN dotnet tool install --global dotnet-ef

# Copiar solo los archivos necesarios para restore
COPY ["EventManagement.Presentation/EventManagement.Presentation.csproj", "EventManagement.Presentation/"]
COPY ["EventManagement.Application/EventManagement.Application.csproj", "EventManagement.Application/"]
COPY ["EventManagement.Domain/EventManagement.Domain.csproj", "EventManagement.Domain/"]
COPY ["EventManagement.Infrastructure/EventManagement.Infrastructure.csproj", "EventManagement.Infrastructure/"]

RUN dotnet restore "EventManagement.Presentation/EventManagement.Presentation.csproj"


COPY . .

WORKDIR "/src/EventManagement.Presentation"
RUN dotnet build "EventManagement.Presentation.csproj" -c Release -o /app/build

# Etapa 2: Publicación
FROM build AS publish
RUN dotnet publish "EventManagement.Presentation.csproj" -c Release -o /app/publish

# Etapa 3: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copiar herramientas EF Core y configurar PATH
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"

# Instalar dependencias para PostgreSQL
RUN apt-get update && \
    apt-get install -y \
    postgresql-client \
    && rm -rf /var/lib/apt/lists/*

# Script de inicio personalizado
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]