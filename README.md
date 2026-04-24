# ProvinciaNetChallenge

## Backend

## Frontend

## 🚀 Cómo ejecutar el proyecto (Docker)

El proyecto está completamente dockerizado. No necesitas instalar el SDK de .NET ni configurar un servidor de SQL Server localmente.

### Requisitos previos

* [Docker Desktop](https://www.docker.com/products/docker-desktop/)  instalado

### Pasos

1. Clonar el repositorio o descargar solo el archivo `docker-compose.yml`.
2. Abrir una terminal en la raíz del proyecto (donde se encuentra el archivo `docker-compose.yml`).
3. Ejecutar el siguiente comando:

   ```bash
   docker-compose up -d
   ```

4. Esperar unos segundos a que la base de datos se inicialice.
5. Navegar a http://localhost:8080/scalar para interactuar con los endpoints
6. Para detener y limpiar los contenedores, ejecuta:

    ```bash
    docker-compose down
    ```

## Stack del proyecto

- C#
- .NET 10
- Clean architecture
- Entity Framework Core
- SQL Server
- Docker
- CI/CD
- Github Actions
  - Para mantener actualizada el contenedor en Docker cloud
  - Para actualizar el sistema en el ambiente
- Unit Test (xUnit)
- Postman
- Normalización de ambientes no productivos (Sandbox)
- JWT
- Patrón repository

## Ambientes

El sistema cuenta con 4 ambientes desplegados en un VPS.

- DEV
- TEST
- PREPROD
- PROD

TODO: Tengo que hacer 4 ambientes de back y de front. Las de back pueden tener su scalar

## Detalle de las pruebas realizadas

## 📂 Estructura del Proyecto

* `MiniETRM.Domain`: Entidades core, Value Objects, Enums y abstracciones de Repositorios/Servicios. No tiene dependencias.
* `MiniETRM.Application`: Casos de uso de negocio y DTOs.
* `MiniETRM.Infrastructure`: Implementación de la persistencia (EF Core DbContext).
* `MiniETRM.Api`: Endpoints RESTful, configuración de Inyección de Dependencias, Middleware y Scalar.
