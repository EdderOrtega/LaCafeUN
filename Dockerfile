# 1. Usar la imagen del SDK de .NET 9.0 para compilar (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar el archivo de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto de los archivos y compilar
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Usar la imagen del Runtime de .NET 9.0 para ejecutar (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Render asigna el puerto automáticamente en la variable de entorno $PORT
# Configurar para escuchar en el puerto proporcionado por Render
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

# Exponer el puerto (Render lo usa dinámicamente)
EXPOSE 8080

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "ProyectoFinalPOO2.dll"]