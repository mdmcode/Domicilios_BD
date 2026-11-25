# 🚚 Domi Express (DomiciliosApp)

Sistema web para la gestión logística de domicilios urbanos desarrollado con **ASP.NET Core MVC (.NET 9.0)**, **Entity Framework Core** y **MySQL 8.0**.

El proyecto implementa un modelo de datos relacional normalizado hasta la **Tercera Forma Normal (3FN)**, con reglas de negocio reforzadas tanto en la base de datos como en la aplicación web, garantizando integridad, trazabilidad y consistencia durante todo el ciclo de vida de los pedidos.

---
## 📋 Descripción

Domi Express permite administrar:

- Clientes y domiciliarios.
- Direcciones de entrega.
- Establecimientos comerciales.
- Solicitudes de domicilio.
- Asignación de repartidores.
- Registro de incidentes.
- Seguimiento del estado de cada pedido.
- Auditoría automática de cambios mediante bitácora histórica.

La plataforma está diseñada para coordinar actores, ubicaciones, pedidos e incidencias dentro de un proceso completo de logística de última milla.

---
## 🏗️ Arquitectura

### Backend

- ASP.NET Core MVC (.NET 9.0)
- Entity Framework Core 9
- Pomelo Entity Framework Core MySQL Provider
- MySQL 8.0

### Frontend

- Razor Views
- JavaScript
- Tom Select
- Bootstrap

### Base de Datos

- Modelo relacional normalizado a 3FN
- Claves primarias simples y compuestas
- Restricciones de integridad referencial
- Funciones almacenadas
- Procedimientos almacenados
- Triggers de validación y auditoría

---
## 📊 Modelo de Datos

El sistema está compuesto por 14 tablas principales:

- usuario
- cliente
- domiciliario
- direccion
- cliente_direccion
- establecimiento
- solicitud
- incidente
- estadoSolicitud
- metodoPago
- medioTransporte
- tipoEstablecimiento
- tipoIncidente
- estadoDomiciliario

Además, existe una tabla de auditoría:
- log_estado_solicitud

---
## 🚦 Ciclo de Vida de una Solicitud

Una solicitud puede pasar por los siguientes estados:

| Estado | Descripción |
|----------|-------------|
| Creada | Pedido registrado y pendiente de asignación |
| Asignada | Domiciliario asignado |
| En Curso | Entrega en progreso |
| Finalizada | Entrega completada |
| Cancelada | Pedido cancelado |

Las transiciones son controladas mediante procedimientos almacenados para garantizar el cumplimiento de las reglas del negocio. 

---
## ⚙️ Reglas de Negocio Implementadas

### Base de Datos

Funciones:

- `fn_total_cliente`
- `fn_domiciliario_disponible`
- `fn_minutos_asignacion`

Procedimientos almacenados:

- `sp_crear_solicitud`
- `sp_asignar_domiciliario`
- `sp_iniciar_entrega`
- `sp_finalizar_solicitud`
- `sp_cancelar_solicitud`
- `sp_registrar_incidente`

Triggers:

- `trg_solicitud_recogida_bi`
- `trg_solicitud_recogida_bu`
- `trg_solicitud_estado_bu`
- `trg_solicitud_log_au`

### Aplicación Web

- Formularios dinámicos según el rol.
- Selectores dependientes para clientes y direcciones.
- Validación de origen único de recogida.
- Gestión visual de estados.
- Manejo amigable de errores SQLSTATE 45000.
- Visualización del historial completo de cambios.

---
## 👥 Gestión de Usuarios

El sistema soporta tres tipos de usuario:

- Cliente
- Domiciliario
- Ambos

La implementación utiliza una estrategia de herencia 1:1 donde los datos comunes se almacenan en la entidad Usuario y los datos específicos en Cliente y Domiciliario. 【1-51396e\|17】【1-51396e\|39】

---
## 📦 Funcionalidades Principales

### Administración de Personas

- Registro de usuarios.
- Gestión de clientes.
- Gestión de domiciliarios.
- Consulta de disponibilidad de repartidores.

### Gestión Geográfica

- Registro de direcciones.
- Asociación cliente-dirección.
- Administración de establecimientos.

### Operación

- Creación de solicitudes.
- Asignación de domiciliarios.
- Inicio y finalización de entregas.
- Cancelación de pedidos.
- Seguimiento del estado del pedido.

### Incidentes

- Registro de novedades.
- Consulta y filtrado de incidentes.
- Trazabilidad completa por solicitud.

---
## 🧪 Pruebas

El proyecto cuenta con una suite automatizada utilizando:

- Postman
- Newman

Se ejecutaron:

- 13 peticiones
- 19 aserciones

Validando tanto escenarios exitosos como casos de error relacionados con las reglas de negocio definidas. 

---

## 🚀 Ejecución del Proyecto

### Requisitos

- .NET SDK 9.0
- MySQL Server 8.0
- Visual Studio 2022 o superior
- Git

### Clonar el repositorio

```bash
git clone https://github.com/mdmcode/Domicilios_BD.git
cd Domicilios_BD
```

### Configurar la cadena de conexión

Editar:

```json
appsettings.json
```

Configurando los datos de acceso a MySQL.

### Restaurar dependencias

```bash
dotnet restore
```

### Ejecutar migraciones o scripts de base de datos

Importar el esquema SQL proporcionado en el proyecto sobre MySQL 8.0.

### Ejecutar la aplicación

```bash
dotnet run
```

Acceso local:

```text
http://localhost:5229
```

---
## 📚 Tecnologías Utilizadas

| Tecnología | Versión |
|------------|----------|
| .NET | 9.0 |
| ASP.NET Core MVC | 9.0 |
| Entity Framework Core | 9.0 |
| MySQL | 8.0 |
| Razor Pages / Views | Incluido |
| Tom Select | Frontend |
| Newman | Testing |

---
## 👨‍💻 Autores

- Iván Fernando Flechas Plaza
- María del Mar Mosquera Caicedo

Universidad Autónoma de Occidente

---
## 📖 Referencias

- Entity Framework Core Documentation
- ASP.NET Core MVC Documentation
- MySQL 8.0 Reference Manual

---
## 🔗 Repositorio

https://github.com/mdmcode/Domicilios_BD
