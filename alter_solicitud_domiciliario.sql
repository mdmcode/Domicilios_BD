-- Active: 1791130450384@@127.0.0.1@3306@domicilios
/*
    Permitir solicitudes sin domiciliario asignado

    id_domiciliario es opcional porque una solicitud puede crearse
    antes de que se le asigne un domiciliario. En consecuencia, debe
    aceptar valores NULL.

    Este comando utiliza la sintaxis de MySQL/MariaDB.
*/

ALTER TABLE solicitud
MODIFY COLUMN id_domiciliario INT NULL;

/*
    La clave foránea existente se conserva:

    FOREIGN KEY (id_domiciliario)
    REFERENCES domiciliario(id_domiciliario)

    Si id_domiciliario tiene un valor, debe existir en domiciliario.
    Si es NULL, la solicitud no tiene domiciliario asignado.
*/
