/* ============================================================
   PROCEDIMIENTOS
   ============================================================ */

-- P1. sp_registrar_incidente(p_solicitud, p_tipo, p_descripcion)
-- Valida que existan la solicitud y el tipo de incidente (SIGNAL si no).
-- Inserta el incidente con fecha y hora actuales y un id nuevo.
DELIMITER $$

CREATE PROCEDURE sp_registrar_incidente(
    IN p_solicitud INT,
    IN p_tipo INT,
    IN p_descripcion VARCHAR(100)
)
BEGIN
    DECLARE v_id INT;

    IF NOT EXISTS (SELECT 1 FROM solicitud WHERE id_solicitud = p_solicitud) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solicitud inexistente';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM tipoIncidente WHERE id_tipoIncidente = p_tipo) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Tipo de incidente inexistente';
    END IF;

    -- Calcula el id siguiente porque id_incidente no es auto_increment
    SELECT IFNULL(MAX(id_incidente), 0) + 1 INTO v_id FROM incidente;

    INSERT INTO incidente(id_incidente, tipoIncidente, descripcion, fecha, hora, id_solicitud)
    VALUES (v_id, p_tipo, p_descripcion, CURRENT_DATE, CURRENT_TIME, p_solicitud);
END $$

DELIMITER ;

-- P2. sp_cancelar_solicitud(p_solicitud)
-- Valida que exista y que no este Finalizada ni Cancelada. La pasa a
-- Cancelada (5). El domiciliario (si lo tenia) se conserva como historial:
-- T2 solo exige domiciliario en los estados 2, 3 y 4, no en el 5.
DELIMITER $$

CREATE PROCEDURE sp_cancelar_solicitud(
    IN p_solicitud INT
)
BEGIN
    DECLARE v_estado INT;

    IF NOT EXISTS (SELECT 1 FROM solicitud WHERE id_solicitud = p_solicitud) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solicitud inexistente';
    END IF;

    SELECT id_estado INTO v_estado
    FROM solicitud
    WHERE id_solicitud = p_solicitud;

    IF v_estado IN (4, 5) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede cancelar una solicitud finalizada o ya cancelada';
    END IF;

    UPDATE solicitud
    SET id_estado = 5
    WHERE id_solicitud = p_solicitud;
END $$

DELIMITER ;

-- P3. sp_asignar_domiciliario(p_solicitud, p_domiciliario)
-- Valida que la solicitud exista y este Creada (1), y que el domiciliario
-- este disponible (F2). En un solo UPDATE: asigna domiciliario,
-- fechaAsignacion, horaAsignacion y pasa a Asignada (2).
DELIMITER $$

CREATE PROCEDURE sp_asignar_domiciliario(
    IN p_solicitud INT,
    IN p_domiciliario INT
)
BEGIN
    DECLARE v_estado INT;

    IF NOT EXISTS (SELECT 1 FROM solicitud WHERE id_solicitud = p_solicitud) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solicitud inexistente';
    END IF;

    SELECT id_estado INTO v_estado
    FROM solicitud
    WHERE id_solicitud = p_solicitud;

    IF v_estado != 1 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solo se pueden asignar solicitudes en estado Creada';
    END IF;

    IF fn_domiciliario_disponible(p_domiciliario) = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El domiciliario no esta disponible';
    END IF;

    UPDATE solicitud
    SET id_domiciliario = p_domiciliario,
        fechaAsignacion = CURRENT_DATE,
        horaAsignacion = CURRENT_TIME,
        id_estado = 2
    WHERE id_solicitud = p_solicitud;
END $$

DELIMITER ;

-- P4. sp_finalizar_solicitud(p_solicitud)
-- Valida que exista y este En Curso (3). La pasa a Finalizada (4).
DELIMITER $$

CREATE PROCEDURE sp_finalizar_solicitud(
    IN p_solicitud INT
)
BEGIN
    DECLARE v_estado INT;

    IF NOT EXISTS (SELECT 1 FROM solicitud WHERE id_solicitud = p_solicitud) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solicitud inexistente';
    END IF;

    SELECT id_estado INTO v_estado
    FROM solicitud
    WHERE id_solicitud = p_solicitud;

    IF v_estado != 3 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solo se pueden finalizar solicitudes En Curso';
    END IF;

    UPDATE solicitud
    SET id_estado = 4
    WHERE id_solicitud = p_solicitud;
END $$

DELIMITER ;

-- P5. sp_crear_solicitud(p_cliente, p_direccionEntrega, p_metodoPago,
--     p_monto, p_descripcion, p_establecimiento, p_direccionExterna, OUT p_id)
-- Valida monto > 0. Calcula el nuevo id (MAX + 1) y lo devuelve por p_id.
-- Inserta con fecha y hora actuales, estado Creada (1) y sin domiciliario.
-- No usa transaccion: es un solo INSERT. La FK compuesta (cliente, direccion)
-- y el trigger T1 rechazan los datos invalidos con sus propios errores.
DELIMITER $$

CREATE PROCEDURE sp_crear_solicitud(
    IN p_cliente INT,
    IN p_direccionEntrega INT,
    IN p_metodoPago INT,
    IN p_monto DECIMAL(10,2),
    IN p_descripcion TEXT,
    IN p_establecimiento INT,
    IN p_direccionExterna INT,
    OUT p_id INT
)
BEGIN
    IF p_monto <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El monto debe ser mayor que cero';
    END IF;

    SELECT IFNULL(MAX(id_solicitud), 0) + 1 INTO p_id FROM solicitud;

    INSERT INTO solicitud(id_solicitud, fechaCreacion, horaCreacion, descripcion,
                          montoTotal, id_estado, id_metodoPago, id_cliente,
                          id_direccionEntrega, id_establecimientoRecogida,
                          id_direccionRecogidaExterna)
    VALUES (p_id, CURRENT_DATE, CURRENT_TIME, p_descripcion,
            p_monto, 1, p_metodoPago, p_cliente,
            p_direccionEntrega, p_establecimiento,
            p_direccionExterna);
END $$

DELIMITER ;