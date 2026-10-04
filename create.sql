/*
    La tabla usuario se puede interpretar como una tabla padre
    de domiciliario y cliente, asi evitamos repetir los atributos
    de nombre, telefono y correo en ambas tablas, y usamos usuario
    para guardar ambos juntos
*/

CREATE TABLE usuario(
    id_usuario INT PRIMARY KEY,
    numeroId INT UNIQUE,
    nombre_completo VARCHAR(255) NOT NULL,
    telefono VARCHAR(10) NOT NULL,
    correo VARCHAR(255) NOT NULL
);

CREATE TABLE cliente(
    id_cliente INT PRIMARY KEY,
    FOREIGN KEY (id_cliente) REFERENCES usuario(id_usuario)
);

CREATE TABLE direccion(
    id_direccion INT PRIMARY KEY,
    ubicacion VARCHAR(500),
    barrio VARCHAR(255),
    comuna VARCHAR(255),
    referencia TEXT NULL
);

/*
    Creamos una tabla intermedia entre cliente y direccion
    porque las direcciones pueden tener mas de un propietario
    y no todas pertenecen unicamente a clientes, tambien pueden
    pertenecer a establecimientos. Solo se crea cliente-direccion
    porque un establecimiento no puede tener varias direcciones
*/

CREATE TABLE cliente_direccion(
    id_cliente INT NOT NULL,
    id_direccion INT NOT NULL,
    PRIMARY KEY (id_cliente, id_direccion),
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
    FOREIGN KEY (id_direccion) REFERENCES direccion(id_direccion)
);

/* 
    Creamos las tablas de estado, metodoPago y medioDeTransporte para cumplir la tercera forma normal, que indica que no deben haber atributos que no dependan unicamente de la llave primaria. Lo mismo aplica para tipoEstablecimiento y tipoIncidente
*/

CREATE TABLE estado(
    id_estado INT PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE metodoPago(
    id_metodoPago INT PRIMARY KEY,
    nombre VARCHAR(255)
);

CREATE TABLE medioDeTransporte(
    id_transporte INT PRIMARY KEY,
    nombre VARCHAR(255)
);

CREATE TABLE domiciliario(
    id_domiciliario INT PRIMARY KEY,
    codigo INT UNIQUE,
    id_transporte INT,
    FOREIGN KEY (id_domiciliario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_transporte) REFERENCES medioDeTransporte(id_transporte)
);

CREATE TABLE tipoEstablecimiento(
    id_tipoEstablecimiento INT PRIMARY KEY,
    nombre VARCHAR(255)
);

CREATE TABLE establecimiento(
    id_establecimiento INT PRIMARY KEY,
    codigo INT UNIQUE,
    nombre VARCHAR(100),
    id_tipoEstablecimiento INT,
    id_direccion INT NOT NULL,
    telefono VARCHAR(100),
    email VARCHAR(100) NULL,
    FOREIGN KEY (id_tipoEstablecimiento) REFERENCES tipoEstablecimiento(id_tipoEstablecimiento),
    FOREIGN KEY (id_direccion) REFERENCES direccion(id_direccion)
);

/*
    Se puede decir que solicitud es la tabla central del sistema de 
    gestión de domicilios
*/

CREATE TABLE solicitud(
    id_solicitud INT PRIMARY KEY,
    fechaCreacion DATE,
    horaCreacion TIME,
    descripcion TEXT,
    montoTotal FLOAT,
    id_estado INT NOT NULL,
    id_metodoPago INT NOT NULL,
    id_cliente INT NOT NULL,
    id_domiciliario INT, -- Tiene multiplicidad 0..*
    fechaAsignacion DATE,
    horaAsignacion TIME,
    id_direccionEntrega INT NOT NULL, --Usamos una direccion relacionada a un cliente
    id_establecimientoRecogida INT NULL, --Si se recoge en un establecimiento
    id_direccionRecogidaExterna INT NULL, --Si se recoge en un lugar externo
    FOREIGN KEY (id_estado) REFERENCES estado(id_estado),
    FOREIGN KEY (id_metodoPago) REFERENCES metodoPago(id_metodoPago),
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
    FOREIGN KEY (id_domiciliario) REFERENCES domiciliario(id_domiciliario),
    FOREIGN KEY (id_cliente, id_direccionEntrega) REFERENCES cliente_direccion(id_cliente, id_direccion), --Hacemos referencia a un par unico de cliente-direccion
    FOREIGN KEY (id_establecimientoRecogida) REFERENCES establecimiento(id_establecimiento),
    FOREIGN KEY (id_direccionRecogidaExterna) REFERENCES direccion(id_direccion)
);


CREATE TABLE tipoIncidente(
    id_tipoIncidente INT PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE incidente(
    id_incidente INT PRIMARY KEY,
    tipoIncidente INT NOT NULL,
    descripcion VARCHAR(100),
    fecha DATE NOT NULL,
    hora TIME NOT NULL,
    id_solicitud INT,
    FOREIGN KEY (tipoIncidente) REFERENCES tipoIncidente(id_tipoIncidente),
    FOREIGN KEY (id_solicitud) REFERENCES solicitud(id_solicitud)
);