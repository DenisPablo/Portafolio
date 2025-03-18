# Usar la imagen del SDK de .NET para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Definir el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copiar el archivo .sln y el archivo .csproj
COPY Portafolio.sln ./
COPY Portafolio/Portafolio.csproj ./Portafolio/

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto de los archivos del proyecto
COPY . ./

# Publicar la aplicación
RUN dotnet publish -c Release -o out

# Usar la imagen de ASP.NET para el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-env

WORKDIR /app

# Definir las variables de entorno necesarias
ENV DB_SERVER=
ENV DB_NAME=
ENV DB_USER=
ENV DB_PASSWORD=

# Copiar la publicación del contenedor build-env
COPY --from=build-env /app/out .

# Establecer el comando de inicio de la aplicación
ENTRYPOINT ["dotnet", "Portafolio.dll"]
