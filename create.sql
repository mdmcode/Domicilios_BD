CREATE TABLE cliente(
    id_cliente INT PRIMARY KEY,
    nombre VARCHAR(500),
    telefono VARCHAR(10),
    correo VARCHAR(500),
    id_direcciones INT,
    FOREIGN KEY (id_direcciones) REFERENCES direccion(id_direccion)
);

CREATE TABLE direccion(
    id_direccion INT PRIMARY KEY,
    ubicacion VARCHAR(500),
    barrio VARCHAR(255),
    comuna VARCHAR(255),
    referencia TEXT NULL
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
)

CREATE TABLE metodoPago(
    id_metodoPago INT PRIMARY KEY,
    nombre VARCHAR(255)
)