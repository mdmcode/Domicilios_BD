/*
    ÍNDICE DE CONSULTAS

    A. CONSULTAS GENERALES
    1. Mostrar establecimientos con tipo y dirección.
    2. Mostrar información de todos los clientes.
    3. Mostrar todos los domiciliarios con usuario, transporte y estado.
    4. Mostrar las direcciones registradas por cada cliente.
    5. Mostrar tablas maestras: estados de solicitud, estados de domiciliarios,
       métodos de pago, medios de transporte, tipos de establecimiento y tipos de incidente.

    B. BÚSQUEDAS POR CATEGORÍAS
    6. Buscar establecimientos por tipo (restaurante, farmacia, supermercado, tienda especializada).
    7. Buscar establecimientos por barrio o comuna.
    8. Buscar cliente por número de identificación.
    9. Buscar domiciliarios activos.
    10. Buscar domiciliarios por medio de transporte (bicicleta, motocicleta, automóvil).

    C. CONSULTAS DE SOLICITUDES
    11. Buscar solicitudes por estado (creada, asignada, en curso, finalizada, cancelada).
    12. Buscar solicitudes realizadas por un cliente.
    13. Mostrar el detalle completo de todas las solicitudes.
    14. Mostrar solicitudes sin domiciliario asignado.
    15. Mostrar solicitudes asignadas a cada domiciliario.
    16. Mostrar el historial de trabajo de un domiciliario.
    17. Mostrar domiciliarios activos sin solicitudes asignadas.

    D. INCIDENTES
    18. Mostrar todos los incidentes relacionados con solicitudes.
    19. Mostrar los incidentes de una solicitud específica.
    20. Mostrar solicitudes que tuvieron incidentes.
    21. Mostrar establecimientos relacionados con incidentes.

    E. SUBCONSULTAS
    22. Clientes que han realizado al menos una solicitud.
    23. Clientes con solicitudes finalizadas.
    24. Clientes con solicitudes superiores a 500.
    25. Domiciliarios que tienen o han tenido solicitudes asignadas.
    26. Clientes que nunca han realizado solicitudes.
    27. Clientes que no han realizado solicitudes finalizadas.
    28. Domiciliarios que no tienen solicitudes asignadas.
    29. Establecimientos que no han sido utilizados en solicitudes.
    30. Clientes con al menos una solicitud (EXISTS).
    31. Clientes con al menos una solicitud superior a 500 (EXISTS).
    32. Clientes con solicitudes pendientes de asignación.
    33. Domiciliarios activos con al menos una solicitud.
    34. Establecimientos con al menos una solicitud asociada.
*/

/* ===== CONSULTAS GENERALES ===== */

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

/* ===== BÚSQUEDA POR ESTABLECIMIENTOS, CLIENTES Y DOMICILIARIOS ===== */

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

/* ===== CONSULTAS DE SOLICITUDES ===== */

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

/* ===== INCIDENTES ===== */

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

/* ===== SUBCONSULTAS ===== */
-- 1. Clientes que han realizado al menos una solicitud.
-- La subconsulta devuelve los id_cliente que aparecen en solicitud.
SELECT
    u.nombre_completo AS "Nombre del cliente",
    u.numeroId AS "Identificación"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE c.id_cliente IN (
    SELECT s.id_cliente
    FROM solicitud s
);


-- 2. Clientes que tienen solicitudes finalizadas.
-- La subconsulta obtiene los clientes relacionados con el estado cuyo nombre es "Finalizada".
SELECT
    u.nombre_completo AS "Nombre del cliente"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE c.id_cliente IN (
    SELECT s.id_cliente
    FROM solicitud s
    JOIN estadosolicitud es
        ON s.id_estado = es.id_estado
    WHERE es.nombre = 'Finalizada'
);


-- 3. Clientes que han solicitado un domicilio con valor superior a 500.
SELECT
    u.nombre_completo AS "Nombre del cliente"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE c.id_cliente IN (
    SELECT s.id_cliente
    FROM solicitud s
    WHERE s.montoTotal > 500
);


-- 4. Domiciliarios que tienen o han tenido solicitudes asignadas.
SELECT
    u.nombre_completo AS "Nombre del domiciliario",
    d.codigo AS "Código"
FROM domiciliario d
JOIN usuario u
    ON d.id_domiciliario = u.id_usuario
WHERE d.id_domiciliario IN (
    SELECT s.id_domiciliario
    FROM solicitud s
    WHERE s.id_domiciliario IS NOT NULL
);

-- 5. Clientes que nunca han realizado solicitudes.
SELECT
    u.nombre_completo AS "Nombre del cliente",
    u.numeroId AS "Identificación"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE c.id_cliente NOT IN (
    SELECT s.id_cliente
    FROM solicitud s
);


-- 6. Clientes que no han realizado solicitudes finalizadas.
SELECT
    u.nombre_completo AS "Nombre del cliente"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE c.id_cliente NOT IN (
    SELECT s.id_cliente
    FROM solicitud s
    JOIN estadosolicitud es
        ON s.id_estado = es.id_estado
    WHERE es.nombre = 'Finalizada'
);


-- 7. Domiciliarios que no tienen solicitudes asignadas.
-- El filtro IS NOT NULL evita que una fila NULL en la subconsulta afecte el resultado de NOT IN.
SELECT
    u.nombre_completo AS "Nombre del domiciliario",
    d.codigo AS "Código"
FROM domiciliario d
JOIN usuario u
    ON d.id_domiciliario = u.id_usuario
WHERE d.id_domiciliario NOT IN (
    SELECT s.id_domiciliario
    FROM solicitud s
    WHERE s.id_domiciliario IS NOT NULL
);


-- 8. Establecimientos que no han sido utilizados en solicitudes.
SELECT
    e.nombre AS "Establecimiento",
    e.codigo AS "Código"
FROM establecimiento e
WHERE e.id_establecimiento NOT IN (
    SELECT s.id_establecimientoRecogida
    FROM solicitud s
    WHERE s.id_establecimientoRecogida IS NOT NULL
);

-- 9. Clientes que tienen al menos una solicitud.
-- La subconsulta está correlacionada con la consulta externa mediante c.id_cliente.
SELECT
    u.nombre_completo AS "Nombre del cliente"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE EXISTS (
    SELECT 1
    FROM solicitud s
    WHERE s.id_cliente = c.id_cliente
);


-- 10. Clientes que tienen al menos una solicitud superior a 500.
SELECT
    u.nombre_completo AS "Nombre del cliente"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE EXISTS (
    SELECT 1
    FROM solicitud s
    WHERE s.id_cliente = c.id_cliente
      AND s.montoTotal > 500
);


-- 11. Clientes que tienen solicitudes pendientes de asignación.
-- Una solicitud pendiente de asignación es aquella cuyo domiciliario todavía no ha sido asignado.
SELECT
    u.nombre_completo AS "Nombre del cliente"
FROM cliente c
JOIN usuario u
    ON c.id_cliente = u.id_usuario
WHERE EXISTS (
    SELECT 1
    FROM solicitud s
    WHERE s.id_cliente = c.id_cliente
      AND s.id_domiciliario IS NULL
);


-- 12. Domiciliarios activos que tienen al menos una solicitud.
SELECT
    u.nombre_completo AS "Nombre del domiciliario",
    d.codigo AS "Código"
FROM domiciliario d
JOIN usuario u
    ON d.id_domiciliario = u.id_usuario
WHERE EXISTS (
    SELECT 1
    FROM estadodomiciliario ed
    WHERE ed.id_estado = d.id_estado
      AND ed.nombre = 'Activo'
)
AND EXISTS (
    SELECT 1
    FROM solicitud s
    WHERE s.id_domiciliario = d.id_domiciliario
);


-- 13. Establecimientos que tienen al menos una solicitud asociada.
SELECT
    e.nombre AS "Establecimiento",
    e.codigo AS "Código"
FROM establecimiento e
WHERE EXISTS (
    SELECT 1
    FROM solicitud s
    WHERE s.id_establecimientoRecogida = e.id_establecimiento
);
