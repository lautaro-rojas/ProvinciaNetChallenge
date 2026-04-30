# 🛠️ ¿Cómo mejoraría el sistema?

- Paginación y Filtrado en la API: Si el inventario crece a 100,000 productos, un GET /Products colapsaría la memoria de la API y la red. Implementaría paginación (usando Skip y Take de LINQ) para devolver resultados por lotes, además de permitir filtrar por SKU o Categoría.

- Estrategia de Caché: El catálogo de productos suele tener muchas más lecturas que escrituras. Implementaría Redis (o un MemoryCache de .NET como primer paso) para no golpear a SQL Server cada vez que alguien entra a ver los productos.

- Refresh Tokens: El JWT actual tiene una fecha de expiración. En un sistema real, no querés que el usuario se tenga que loguear con usuario y contraseña cada 30 minutos. Agregaría un endpoint para rotar el token usando un Refresh Token guardado en la base de datos.

- Observabilidad Centralizada: Los Console.WriteLine están bien para arrancar, pero en la nube necesitas trazabilidad. Integraría Serilog para generar logs estructurados y enviarlos a un panel como Seq, Datadog o Application Insights.

- Manejo de Transacciones (Unit of Work): Si el servicio realiza múltiples operaciones de escritura en distintos repositorios dentro del mismo caso de uso, implementaría el patrón Unit of Work para asegurar que todo se guarde (commit) o se revierta (rollback) de forma atómica.
