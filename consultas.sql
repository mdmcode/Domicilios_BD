-- Mostramos los establecimientos con sus respectivos tipo y direcciones
SELECT 
    e.codigo, 
    e.nombre, 
    e.telefono, 
    e.email, 
    tipo.nombre AS 'Tipo establecimiento',
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion;

-- Mostramos la información de todos los clientes relacionando cliente con usuario
SELECT 
    u.nombre_completo AS 'Nombre',
    u.numeroId AS 'Identificación',
    u.telefono AS 'Telefono',
    u.correo AS 'Correo'
FROM cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario;

-- Mostramos todos los domiciliarios relacionando domiciliario con usuario, medioDeTransporte y estadoDomiciliario
SELECT 
    dom.codigo,
    u.nombre_completo AS 'Nombre',
    u.numeroId AS 'Identificación',
    u.telefono AS 'Telefono',
    u.correo AS 'Correo',
    medio.nombre AS 'Medio de transporte',
    e.nombre AS 'Estado'
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado;

-- Mostramos todas las direcciones registradas por cada cliente, relacionando cliente con usuario, cliente_direccion y direccion
SELECT 
    u.nombre_completo AS 'Nombre',
    u.numeroId AS 'Identificación',
    u.telefono AS 'Telefono',
    u.correo AS 'Correo',
    d.id_direccion,
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM cliente
JOIN cliente_direccion cd ON cd.id_cliente = cliente.id_cliente
JOIN direccion d ON cd.id_direccion = d.id_direccion
JOIN usuario u ON cliente.id_cliente = u.id_usuario;

-- Mostramos la información de las tablas mas pequeñas (estadoSolicitud, estadoDomiciliario, metodoDePago, medioDeTransporte, tipoEstablecimiento, tipoIncidente)
SELECT * FROM estadosolicitud;
SELECT * FROM estadodomiciliario;
SELECT * FROM metododepago;
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
    tipo.nombre AS 'Tipo establecimiento',
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = 'Restaurante';

-- Farmacias
SELECT
    e.codigo,
    e.nombre,
    e.telefono,
    e.email,
    tipo.nombre AS 'Tipo establecimiento',
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = 'Farmacia';

-- Supermercados
SELECT
    e.codigo,
    e.nombre,
    e.telefono,
    e.email,
    tipo.nombre AS 'Tipo establecimiento',
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = 'Supermercado';

-- Tiendas especializadas
SELECT
    e.codigo,
    e.nombre,
    e.telefono,
    e.email,
    tipo.nombre AS 'Tipo establecimiento',
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE tipo.nombre = 'Tienda especializada';

-- Buscar establecimiento por barrio o comuna
SELECT
    e.codigo, 
    e.nombre, 
    e.telefono, 
    e.email, 
    tipo.nombre AS 'Tipo establecimiento',
    d.ubicacion AS 'Ubicación',
    d.barrio AS 'Barrio',
    d.comuna AS 'Comuna',
    d.referencia AS 'Referencia'
FROM establecimiento e
JOIN tipoestablecimiento tipo ON e.id_tipoEstablecimiento = tipo.id_tipoEstablecimiento
JOIN direccion d ON e.id_direccion = d.id_direccion
WHERE d.barrio = '' OR d.comuna = '';

-- Buscar cliente por numero de identificación
SELECT 
    u.nombre_completo AS 'Nombre',
    u.numeroId AS 'Identificación',
    u.telefono AS 'Telefono',
    u.correo AS 'Correo'
FROM cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario
WHERE u.numeroId = '';

-- Buscar domiciliarios activos
SELECT 
    dom.codigo,
    u.nombre_completo AS 'Nombre',
    u.numeroId AS 'Identificación',
    u.telefono AS 'Telefono',
    u.correo AS 'Correo',
    medio.nombre AS 'Medio de transporte',
    e.nombre AS 'Estado'
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado
WHERE e.id_estado = 1;

-- Buscar domiciliarios por medio de transporte
SELECT 
    dom.codigo,
    u.nombre_completo AS 'Nombre',
    u.numeroId AS 'Identificación',
    u.telefono AS 'Telefono',
    u.correo AS 'Correo',
    medio.nombre AS 'Medio de transporte',
    e.nombre AS 'Estado'
FROM domiciliario dom
JOIN usuario u ON dom.id_domiciliario = u.id_usuario
JOIN mediodetransporte medio ON dom.id_transporte = medio.id_transporte
JOIN estadodomiciliario e ON dom.id_estado = e.id_estado
-- Encuentra los que van en bicicleta por ejemplo
WHERE medio.id_transporte = 1; 

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
WHERE e.nombre = 'Creada';

-- Buscar solicitudes realizadas por un cliente
SELECT 
    s.id_solicitud,
    s.fechaCreacion,
    s.horaCreacion,
    s.descripcion,
    s.montoTotal,
    u.nombre_completo AS 'Nombre del cliente',
    u.numeroId AS 'Identificación del cliente'
FROM solicitud s
JOIN cliente ON s.id_cliente = cliente.id_cliente
JOIN usuario u ON cliente.id_cliente = u.id_usuario
WHERE u.numeroId = ''; 

/*
    La consulta principal del sistema permite mostrar todas las solicitudes a detalle,
    y relaciona Solicitud con cliente, usuario, direccion, establecimiento, domiciliario,
    estadoSolicitud, metodoPago
*/


-- Mostramos solicitudes que todavía no tienen domiciliario asignado (teniendo en cuenta la multiplicidad 0..* que tiene domiciliario)

-- Mostramos las solicitudes asignadas a cada domiciliario

-- Contamos cuantas solicitudes ha atendido cada domiciliario (opcional)

-- Mostramos el historial de trabajo de un domiciliario

-- Mostramos que domiciliarios activos no tienen solicitudes asignadas

-- Consultamos las solicitudes sin asignación despues de cierto tiempo


-- Mostramos todos los incidentes relacionados con solicitudes

-- Mostramos los incidentes de una solicitud específica

-- Contamos incidentes por tipo (opcional)

-- Mostramos que solicitudes tuvieron incidentes

-- Mostramos los incidentes ocurridos en un periodo de tiempo (usando fecha)

-- Mostramos los establecimientos relacionados con incidentes