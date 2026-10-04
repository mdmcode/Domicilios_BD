-- Mostramos los establecimientos con sus respectivos tipo y direcciones
SELECT 
    e.codigo, 
    e.nombre, 
    e.telefono, 
    e.email, 
    tipo.nombre AS "Tipo establecimiento",
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion;

-- Mostramos la información de todos los clientes relacionando cliente con usuario
SELECT 
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo"
FROM cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario;

-- Mostramos todos los domiciliarios relacionando domiciliario con usuario, medioDeTransporte y estadoDomiciliario
SELECT 
    dom.codigo,
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo",
    medio.nombre AS "Medio de transporte",
    e.nombre AS "Estado"
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado;

-- Mostramos todas las direcciones registradas por cada cliente, relacionando cliente con usuario, cliente_direccion y direccion
SELECT 
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo",
    d.id_direccion,
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM cliente
JOIN cliente_direccion cd ON cd.id_cliente = cliente.id_cliente
JOIN direccion d ON cd.id_direccion = d.id_direccion
JOIN usuario u ON cliente.id_cliente = u.id_usuario;

-- Mostramos la información de las tablas mas pequeñas (estadoSolicitud, estadoDomiciliario, metodoDePago, medioDeTransporte, tipoEstablecimiento, tipoIncidente)
SELECT * FROM estadosolicitud;
SELECT * FROM estadodomiciliario;
SELECT * FROM metodoPago;
SELECT * FROM mediodetransporte;
SELECT * FROM tipoestablecimiento;
SELECT * FROM tipoincidente;

-- Buscar establecimiento por tipo
-- Restaurantes
SELECT
    e.codigo, 
    e.nombre, 
    e.telefono, 
    e.email, 
    tipo.nombre AS "Tipo establecimiento",
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = "Restaurante";

-- Farmacias
SELECT
    e.codigo,
    e.nombre,
    e.telefono,
    e.email,
    tipo.nombre AS "Tipo establecimiento",
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = "Farmacia";

-- Supermercados
SELECT
    e.codigo,
    e.nombre,
    e.telefono,
    e.email,
    tipo.nombre AS "Tipo establecimiento",
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = "Supermercado";

-- Tiendas especializadas
SELECT
    e.codigo,
    e.nombre,
    e.telefono,
    e.email,
    tipo.nombre AS "Tipo establecimiento",
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = "Tienda especializada";

-- Buscar establecimiento por barrio o comuna
SELECT
    e.codigo, 
    e.nombre, 
    e.telefono, 
    e.email, 
    tipo.nombre AS "Tipo establecimiento",
    d.ubicacion AS "Ubicación",
    d.barrio AS "Barrio",
    d.comuna AS "Comuna",
    d.referencia AS "Referencia"
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE d.barrio = "Limonar" OR d.comuna = "Comuna 17";

-- Buscar cliente por numero de identificación
SELECT 
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo"
FROM cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario
WHERE u.numeroId = "1097061525";

-- Buscar domiciliarios activos
SELECT 
    dom.codigo,
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo",
    medio.nombre AS "Medio de transporte",
    e.nombre AS "Estado"
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado
WHERE e.id_estado = 1;

-- Buscar domiciliarios por medio de transporte
-- Encuentra los que van en bicicleta
SELECT 
    dom.codigo,
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo",
    medio.nombre AS "Medio de transporte",
    e.nombre AS "Estado"
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado
WHERE medio.nombre = "Bicicleta";

-- Encuentra los que van en motocicleta
SELECT 
    dom.codigo,
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo",
    medio.nombre AS "Medio de transporte",
    e.nombre AS "Estado"
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado
WHERE medio.nombre = "Motocicleta";

-- Encuentra los que van en automovil
SELECT 
    dom.codigo,
    u.nombre_completo AS "Nombre",
    u.numeroId AS "Identificación",
    u.telefono AS "Telefono",
    u.correo AS "Correo",
    medio.nombre AS "Medio de transporte",
    e.nombre AS "Estado"
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado
WHERE medio.nombre = "Automovil";

-- Buscar solicitudes por estado
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
WHERE e.nombre = "Creada";

-- Asignadas
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
WHERE e.nombre = "Asignada";

-- En curso
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
WHERE e.nombre = "En curso";

-- Finalizadas
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
WHERE e.nombre = "Finalizada";

-- Canceladas
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
WHERE e.nombre = "Cancelada";

-- Buscar solicitudes realizadas por un cliente
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    u.nombre_completo AS "Nombre del cliente",
    u.numeroId AS "Identificación del cliente"
FROM solicitud s
JOIN cliente ON s.id_cliente = cliente.id_cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario
WHERE u.numeroId = "1097061525"; 

/*
    La consulta principal del sistema permite mostrar todas las solicitudes a detalle,
    y relaciona Solicitud con cliente, usuario, direccion, establecimiento, domiciliario,
    estadoSolicitud, metodoPago
*/
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre AS "Estado",
    me.nombre AS "Metodo de Pago",
    -- Datos del cliente
    uc.nombre_completo AS "Nombre Cliente",
    uc.numeroId AS "Identificacion Cliente",
    uc.telefono AS "Telefono Cliente",
    uc.correo AS "Correo Cliente",
    -- Datos de la direccion de entrega
    de.ubicacion AS "Ubicacion Entrega",
    de.barrio AS "Barrio Entrega",
    de.comuna AS "Comuna Entrega",
    de.referencia AS "Referencia Entrega",
    -- Datos del domiciliario
    dom.codigo AS "Codigo Domiciliario",
    ud.nombre_completo AS "Nombre Domiciliario",
    ud.numeroId AS "Identificacion Domiciliario",
    ud.telefono AS "Telefono Domiciliario",
    ud.correo AS "Correo Domiciliario",
    medio.nombre AS "Medio de Transporte",
    ed.nombre AS "Estado Domiciliario",
    s.fechaAsignacion AS "Fecha Asignacion",
    s.horaAsignacion AS "Hora Asignacion",
    -- Lugar de recogida en un establecimiento
    est.nombre AS "Establecimiento Recogida",
    tipoEst.nombre AS "Tipo Establecimiento",
    deest.ubicacion AS "Ubicacion Establecimiento",
    deest.barrio AS "Barrio Establecimiento",
    deest.comuna AS "Comuna Establecimiento",
    -- Lugar de recogida externo
    dext.ubicacion AS "Ubicacion Recogida Externa",
    dext.barrio AS "Barrio Recogida Externa",
    dext.comuna AS "Comuna Recogida Externa",
    dext.referencia AS "Referencia Recogida Externa"
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
JOIN metodopago me ON s.id_metodoPago = me.id_metodoPago
JOIN cliente ON s.id_cliente = cliente.id_cliente
JOIN usuario uc ON cliente.id_cliente = uc.id_usuario
-- Buscamos la relacion en la que coincidan simultaneamtente el cliente y la direccion de entrega
JOIN cliente_direccion cd
    ON s.id_cliente = cd.id_cliente
    AND s.id_direccionEntrega = cd.id_direccion
JOIN direccion de ON cd.id_direccion = de.id_direccion
LEFT JOIN domiciliario dom ON s.id_domiciliario = dom.id_domiciliario
LEFT JOIN usuario ud ON dom.id_domiciliario = ud.id_usuario
LEFT JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
LEFT JOIN estadodomiciliario ed ON dom.id_estado = ed.id_estado
LEFT JOIN establecimiento est ON s.id_establecimientoRecogida = est.id_establecimiento
LEFT JOIN tipoestablecimiento tipoEst 
    ON est.id_tipoEstablecimiento = tipoEst.id_tipoEstablecimiento
-- Direccion para establecimiento
LEFT JOIN direccion deest ON est.id_direccion = deest.id_direccion
-- Direccion para recogida externa
LEFT JOIN direccion dext ON s.id_direccionRecogidaExterna = dext.id_direccion;

-- Mostramos solicitudes que todavía no tienen domiciliario asignado (teniendo en cuenta la multiplicidad 0..1 que tiene domiciliario)
SELECT 
    s.id_solicitud AS "Número de solicitud",
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    e.nombre AS 'Estado',
    u.nombre_completo AS "Nombre del cliente",
    u.numeroId AS "Identificación del cliente"
FROM solicitud s
JOIN estadosolicitud e ON s.id_estado = e.id_estado
JOIN cliente ON s.id_cliente = cliente.id_cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario
WHERE s.id_domiciliario IS NULL; 

-- Mostramos las solicitudes asignadas a cada domiciliario
SELECT 
    dom.codigo AS "Codigo del domiciliario",
    u.nombre_completo AS "Nombre del domiciliario",
    s.id_solicitud AS "Número de solicitud",
    s.fechaCreacion,
    s.horaCreacion,
    e.nombre AS "Estado",
    s.descripcion,
    s.montoTotal
FROM solicitud s
JOIN domiciliario dom ON s.id_domiciliario = dom.id_domiciliario
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN estadosolicitud e ON s.id_estado = e.id_estado;

-- Mostramos el historial de trabajo de un domiciliario
SELECT
    dom.codigo AS "Codigo del domiciliario",
    u.nombre_completo AS "Nombre del domiciliario",
    s.id_solicitud AS "Numero de solicitud",
    s.fechaCreacion,
    s.horaCreacion,
    s.fechaAsignacion,
    s.horaAsignacion,
    e.nombre AS "Estado",
    s.descripcion,
    s.montoTotal
FROM solicitud s
JOIN domiciliario dom ON s.id_domiciliario = dom.id_domiciliario
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN estadosolicitud e ON s.id_estado = e.id_estado
WHERE dom.codigo = '101';

-- Mostramos que domiciliarios activos no tienen solicitudes asignadas
SELECT
    dom.codigo AS "Codigo del domiciliario",
    u.nombre_completo AS "Nombre del domiciliario",
    u.telefono AS "Telefono",
    u.correo AS "Correo"
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN estadodomiciliario ed ON dom.id_estado = ed.id_estado
LEFT JOIN solicitud s ON dom.id_domiciliario = s.id_domiciliario
WHERE ed.id_estado = 1
  AND s.id_solicitud IS NULL;

-- Mostramos todos los incidentes relacionados con solicitudes
SELECT
    i.id_incidente,
    i.id_solicitud AS "Numero de solicitud",
    ti.nombre AS "Tipo de incidente",
    i.descripcion,
    i.fecha,
    i.hora
FROM incidente i
JOIN tipoincidente ti ON i.tipoIncidente = ti.id_tipoIncidente
ORDER BY i.fecha DESC, i.hora DESC;

-- Mostramos los incidentes de una solicitud específica
SELECT
    i.id_incidente,
    i.id_solicitud AS "Numero de solicitud",
    ti.nombre AS "Tipo de incidente",
    i.descripcion,
    i.fecha,
    i.hora
FROM incidente i
JOIN tipoincidente ti ON i.tipoIncidente = ti.id_tipoIncidente
WHERE i.id_solicitud = 1;

-- Mostramos que solicitudes tuvieron incidentes
SELECT
    s.id_solicitud AS "Numero de solicitud",
    s.fechaCreacion,
    s.descripcion,
    e.nombre AS "Estado"
FROM solicitud s
JOIN incidente i ON s.id_solicitud = i.id_solicitud
JOIN estadosolicitud e ON s.id_estado = e.id_estado;

-- Mostramos los establecimientos relacionados con incidentes
SELECT DISTINCT
    est.codigo,
    est.nombre AS "Establecimiento",
    est.telefono,
    est.email,
    i.id_incidente,
    i.id_solicitud AS "Numero de solicitud",
    ti.nombre AS "Tipo de incidente",
    i.descripcion,
    i.fecha,
    i.hora
FROM incidente i
JOIN solicitud s ON i.id_solicitud = s.id_solicitud
JOIN establecimiento est
    ON s.id_establecimientoRecogida = est.id_establecimiento
JOIN tipoincidente ti ON i.tipoIncidente = ti.id_tipoIncidente;