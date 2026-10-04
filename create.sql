

/* La tabla dirección tiene una relación de uno a muchos con la tabla cliente, un cliente puede tener varias direcciones, entonces la llave foránea queda en la tabla de direcion*/
CREATE TABLE cliente(
    id_cliente INT PRIMARY KEY,
    numeroId INT,
    nombre VARCHAR(500),
    telefono VARCHAR(10),
    correo VARCHAR(500)
);

CREATE TABLE direccion(
    id_direccion INT PRIMARY KEY,
    id_cliente INT NOT NULL,
    ubicacion VARCHAR(500),
    barrio VARCHAR(255),
    comuna VARCHAR(255),
    referencia TEXT NULL,
    --Creamos un par unico direccion-cliente para poder enlazar una direccion especifica con la direccionEntrega en solicitud --
    UNIQUE(id_cliente, id_direccion),
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente)
);

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
    numeroId INT UNIQUE,
    nombre VARCHAR(255),
    telefono VARCHAR(10),
    correo VARCHAR(255),
    id_transporte INT,
    FOREIGN KEY (id_transporte) REFERENCES medioDeTransporte(id_transporte)
);

CREATE TABLE solicitud(
    id_solicitud INT PRIMARY KEY,
    fechaCreacion DATE,
    horaCreacion TIME,
    descripcion VARCHAR(100),
    montoTotal FLOAT,
    id_estado INT NOT NULL,
    id_metodoPago INT NOT NULL,
    id_cliente INT NOT NULL,
    id_domiciliario INT,
    id_direccionEntrega INT NOT NULL, --Usamos una direccion relacionada a un cliente--
    direccionRecogidaExterna VARCHAR(100),
    FOREIGN KEY (id_estado) REFERENCES estado(id_estado),
    FOREIGN KEY (id_metodoPago) REFERENCES metodoPago(id_metodoPago),
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
    FOREIGN KEY (id_domiciliario) REFERENCES domiciliario(id_domiciliario),
    FOREIGN KEY (id_cliente, id_direccionEntrega) REFERENCES direccion(id_cliente, id_direccion) --Hacemos referencia al par unico de cliente-direccion--
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
    id_direccion INT,
    telefono VARCHAR(100),
    email VARCHAR(100) NULL,
    FOREIGN KEY (id_tipoEstablecimiento) REFERENCES tipoEstablecimiento(id_tipoEstablecimiento),
    FOREIGN KEY (id_direccion) REFERENCES direccion(id_direccion)
);

CREATE TABLE tipoIncidente(
    id_tipoIncidente INT PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE incidente(
    id_incidente INT PRIMARY KEY,
    tipoIncidente INT,
    descripcion VARCHAR(100),
    fecha DATE,
    hora TIME,
    id_solicitud INT,
    FOREIGN KEY (tipoIncidente) REFERENCES tipoIncidente(id_tipoIncidente),
    FOREIGN KEY (id_solicitud) REFERENCES solicitud(id_solicitud)
);

CREATE TABLE usuario(
    id INT PRIMARY KEY,
    nombre_completo VARCHAR(255),
    telefono VARCHAR(10),
    correo VARCHAR(255)
);
