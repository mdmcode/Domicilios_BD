CREATE TABLE solicitud(
    id_solicitud INT PRIMARY KEY,
    fechaCreacion DATE,
    horaCreacion TIME,
    estado INT,
    descripcion VARCHAR(100),
    metodoPago INT,
    montoTotal FLOAT,
    direccionRecogidaExterna VARCHAR(100) NOT NULL,
    direccionEntrega VARCHAR(100),
    FOREIGN KEY (estado) REFERENCES estado(id_estado),
    FOREIGN KEY (metodoPago) REFERENCES metodoPago(id)
);

CREATE TABLE estado(
    id_estado INT PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE metodoPago(
    id_metodoPago INT PRIMARY KEY,
    nombre VARCHAR(255)
);

CREATE TABLE direccion(
    id_direccion INT PRIMARY KEY,
    ubicacion VARCHAR(500),
    barrio VARCHAR(255),
    comuna VARCHAR(255),
    referencia TEXT NULL
);

CREATE TABLE establecimiento(
    id_establecimiento INT PRIMARY KEY,
    nombre VARCHAR(100),
    id_tipoEstablecimiento INT,
    id_direccion INT,
    telefono VARCHAR(100),
    email VARCHAR(100) NULL,
    FOREIGN KEY (id_tipoEstablecimiento) REFERENCES tipoEstablecimiento(id_tipoEstablecimiento),
    FOREIGN KEY (id_direccion) REFERENCES direccion(id_direccion)
);

CREATE TABLE cliente(
    id_cliente INT PRIMARY KEY,
    nombre VARCHAR(500),
    telefono VARCHAR(10),
    correo VARCHAR(500),
    id_direcciones INT,
    FOREIGN KEY (id_direcciones) REFERENCES direccion(id_direccion)
);

CREATE TABLE tipoEstablecimiento(
    id_tipoEstablecimiento INT PRIMARY KEY,
    nombre VARCHAR(255)
);

CREATE TABLE incidente(
    id_incidente INT PRIMARY KEY,
    tipoIncidente INT,
    descripcion VARCHAR(100),
    fecha DATE,
    hora TIME,
    FOREIGN KEY (tipoIncidente) REFERENCES tipoIncidente(id_tipoIncidente)
);

CREATE TABLE tipoIncidente(
    id_tipoIncidente INT PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE usuario(
    id INT PRIMARY KEY,
    nombre_completo VARCHAR(255),
    telefono VARCHAR(10),
    correo VARCHAR(255)
);

CREATE TABLE domiciliario(
    id_domiciliario INT PRIMARY KEY,
    codigo INT,
    nombre VARCHAR(255),
    telefono VARCHAR(10),
    correo VARCHAR(255),
    id_transporte INT,
    FOREIGN KEY (id_transporte) REFERENCES medioDeTransporte(id_transporte)
);

CREATE TABLE medioDeTransporte(
    id_transporte INT PRIMARY KEY,
    nombre VARCHAR(255)
);
