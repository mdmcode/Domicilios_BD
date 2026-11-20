USE domicilios;

/* ============================================================
   FUNCIONES
   ============================================================ */

-- F1. fn_total_cliente(p_cliente) -> DECIMAL(12,2)
-- Devuelve el total gastado por el cliente sumando solo las solicitudes
-- Finalizadas (estado 4). IFNULL convierte en 0 el caso sin finalizadas.
DELIMITER $$

CREATE FUNCTION fn_total_cliente(p_cliente INT)
RETURNS DECIMAL(12,2)
READS SQL DATA
BEGIN
    DECLARE v_total DECIMAL(12,2);

    SELECT IFNULL(SUM(montoTotal), 0) INTO v_total
    FROM solicitud
    WHERE id_cliente = p_cliente
      AND id_estado = 4;

    RETURN v_total;
END $$

DELIMITER ;

-- F2. fn_domiciliario_disponible(p_dom) -> TINYINT(1)
-- Devuelve 1 si el domiciliario esta Activo (estado 1) y no tiene
-- solicitudes Asignadas (2) ni En Curso (3). En otro caso, devuelve 0.
DELIMITER $$

CREATE FUNCTION fn_domiciliario_disponible(p_dom INT)
RETURNS TINYINT(1)
READS SQL DATA
BEGIN
    DECLARE v_estado INT;
    DECLARE v_n_solicitudes INT;

    SELECT id_estado INTO v_estado
    FROM domiciliario
    WHERE id_domiciliario = p_dom;

    SELECT COUNT(*) INTO v_n_solicitudes
    FROM solicitud
    WHERE id_domiciliario = p_dom
      AND id_estado IN (2, 3);

    IF v_n_solicitudes = 0 AND v_estado = 1 THEN
        RETURN 1;
    ELSE
        RETURN 0;
    END IF;
END $$

DELIMITER ;

-- F3. fn_minutos_asignacion(p_solicitud) -> INT
-- Devuelve los minutos entre la creacion y la asignacion de la solicitud.
-- Si aun no fue asignada, devuelve NULL.
DELIMITER $$

CREATE FUNCTION fn_minutos_asignacion(p_solicitud INT)
RETURNS INT
READS SQL DATA
BEGIN
    DECLARE v_creacion DATETIME;
    DECLARE v_asignacion DATETIME;

    -- TIMESTAMP(fecha, hora) une ambas columnas; si la fecha/hora es NULL, da NULL
    SELECT TIMESTAMP(fechaCreacion, horaCreacion),
           TIMESTAMP(fechaAsignacion, horaAsignacion)
    INTO v_creacion, v_asignacion
    FROM solicitud
    WHERE id_solicitud = p_solicitud;

    IF v_asignacion IS NULL THEN
        RETURN NULL;
    ELSE
        RETURN TIMESTAMPDIFF(MINUTE, v_creacion, v_asignacion);
    END IF;
END $$

DELIMITER ;