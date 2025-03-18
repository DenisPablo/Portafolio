# Portafolio Personal

###### C# - .NET 8.0 - Docker - SQL Server 2022

Este proyecto permite publicar y administrar proyectos personales, permitiendo asignarle categoria, tecnologias e images que luego seran mostradas en una galeria.

Para desplegar el proyecto:

### Desplegar base de datos

```Bash
# Ingresar a BD editar el docker-compose e ingresar la nueva contraseña y desplegar

docker-compose up -d
```
##### Restaurar el esquema de la bases de datos y los store procedure.
___

### Desplegar la  APP (Portafolio)

```bash

# Crear imagen del proyecto (APP)

docker build -t portafolio .
```

```bash
# Ingresar los datos en docker-compose y desplegar el contenedor de la aplicacion

docker-compose up -d
```
