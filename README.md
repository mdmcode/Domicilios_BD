# Domicilios_BD

Base de datos relacional para gestionar domicilios, clientes, direcciones, establecimientos, domiciliarios, solicitudes e incidentes.

## Contenido

- `create.sql`: crea las tablas, claves primarias y relaciones.
- `insert.sql`: carga datos de ejemplo.
- `consultas.sql`: contiene consultas generales, busquedas,solicitudes, incidentes y subconsultas.
- `Documentacion_Domicilios_BD.docx`: resumen de la estructura y funcionamiento.

## Modelo resumido

- `usuario` centraliza los datos personales de clientes y domiciliarios.
- `cliente_direccion` relaciona clientes con una o varias direcciones.
- `establecimiento` registra lugares de recogida y su tipo.
- `solicitud` es la entidad principal: relaciona cliente, direccion de entrega, pago, estado, recogida y domiciliario.
- `incidente` registra novedades asociadas a una solicitud.

Los catalogos de estados, pagos, transportes y tipos se mantienen en tablas separadas para evitar duplicidad y conservar la integridad referencial.