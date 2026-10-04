INSERT INTO medioDeTransporte(id?trasporte, nombre)
VALUES
    (1, 'Bicicleta'),
    (2, 'Motocicleta'),
    (3, 'Automovil');

INSERT INTO metodoDePago(id_metodoPago, nombre)
VALUES
    (1, 'Efectivo'),
    (2, 'Electronico'),
    (3, 'Ninguno');

INSERT INTO estado(id_estado, nombre)
VALUES
    (1, 'Creada'),
    (2, 'Asignada'),
    (3, 'En Curso'),
    (4, 'Finalizada'),
    (5, 'Cancelada'),
    -- Estos últimos estados son para el domiciliario
    (6, 'Activo'),
    (7, 'Inactivo');        

INSERT INTO tipoEstablecimiento(id_tipoEstablecimiento, nombre)
VALUES
    (1, 'Restaurante'),
    (2, 'Farmacia'),
    (3, 'Supermercado'),
    (4, 'Tienda Especializada');

INSERT INTO tipoIncidente(id_tipoIncidente, nombre)
VALUES
    (1, 'Retraso'),
    (2, 'Dirección Incorrecta'),
    (3, 'Cliente no responde'),
    (4, 'Establecimiento no entrega');