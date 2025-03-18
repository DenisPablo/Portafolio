# Portafolio Personal

###### C# - .NET 8.0 - Docker - SQL Server 2022

Este proyecto permite publicar y administrar proyectos personales, permitiendo asignarle categoria, tecnologias e images que luego seran mostradas en una galeria.

Para desplegar el proyecto:

```Bash
# Ingresar a BD editar el docker-compose e ingresar la nueva contraseña y desplegar

docker-compose up -d
```

```bash

# Crear imagen del proyecto (APP)

docker build -t portafolio .
```

```bash
# Desplegar el contenedor de la aplicacion

docker-compose up -d
```
