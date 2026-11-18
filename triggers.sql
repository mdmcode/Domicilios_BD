USE domicilios;

/* ============================================================
   TRIGGERS
   ============================================================ */

-- T1. trg_solicitud_recogida_bi (BEFORE INSERT ON solicitud)
-- Exige exactamente uno: id_establecimientoRecogida o
-- id_direccionRecogidaExterna. Si vienen ambos o ninguno, bloquea con SIGNAL.
DELIMITER $$

CREATE TRIGGER trg_solicitud_recogida_bi
BEFORE INSERT ON solicitud
FOR EACH ROW
BEGIN
    IF NEW.id_establecimientoRecogida IS NULL AND NEW.id_direccionRecogidaExterna IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solicitud sin direccion de recogida';
    ELSEIF NEW.id_establecimientoRecogida IS NOT NULL AND NEW.id_direccionRecogidaExterna IS NOT NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No puede tener dos direcciones de recogida al mismo tiempo';
    END IF;
END $$

DELIMITER ;

-- T1 (UPDATE). trg_solicitud_recogida_bu (BEFORE UPDATE ON solicitud)
-- Misma regla de recogida, para que no se rompa al modificar una solicitud.
DELIMITER $$

CREATE TRIGGER trg_solicitud_recogida_bu
BEFORE UPDATE ON solicitud
FOR EACH ROW
BEGIN
    IF NEW.id_establecimientoRecogida IS NULL AND NEW.id_direccionRecogidaExterna IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Solicitud sin direccion de recogida';
    ELSEIF NEW.id_establecimientoRecogida IS NOT NULL AND NEW.id_direccionRecogidaExterna IS NOT NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No puede tener dos direcciones de recogida al mismo tiempo';
    END IF;
END $$

DELIMITER ;

-- T2. trg_solicitud_estado_bu (BEFORE UPDATE ON solicitud)
-- 1) Una solicitud Finalizada (4) o Cancelada (5) no cambia de estado.
-- 2) Los estados 2, 3 y 4 exigen id_domiciliario NO NULL.
-- 3) Si cambia el domiciliario, el nuevo debe estar Activo.
DELIMITER $$

CREATE TRIGGER trg_solicitud_estado_bu
BEFORE UPDATE ON solicitud
FOR EACH ROW
BEGIN
    DECLARE v_estadoDomiciliario INT;

    IF OLD.id_estado IN (4, 5) AND NEW.id_estado != OLD.id_estado THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Las solicitudes finalizadas o canceladas no pueden cambiar de estado';
    END IF;

    IF NEW.id_estado IN (2, 3, 4) AND NEW.id_domiciliario IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Es necesario asignarle un domiciliario a las solicitudes';
    END IF;

    -- <=> compara igual que = pero tratando NULL como un valor
    IF NOT (OLD.id_domiciliario <=> NEW.id_domiciliario)
       AND NEW.id_domiciliario IS NOT NULL THEN

        SELECT id_estado INTO v_estadoDomiciliario
        FROM domiciliario
        WHERE id_domiciliario = NEW.id_domiciliario;

        IF v_estadoDomiciliario != 1 THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El domiciliario asignado debe estar activo';
        END IF;
    END IF;
END $$

DELIMITER ;

-- T3. trg_solicitud_log_au (AFTER UPDATE ON solicitud)
-- Inserta una fila en log_estado_solicitud solo cuando id_estado cambie.
CREATE TABLE log_estado_solicitud(
    id INT AUTO_INCREMENT PRIMARY KEY,
    id_solicitud INT,
    estado_anterior INT,
    estado_nuevo INT,
    fecha_cambio DATE
);

DELIMITER $$

CREATE TRIGGER trg_solicitud_log_au
AFTER UPDATE ON solicitud
FOR EACH ROW
BEGIN
    IF OLD.id_estado != NEW.id_estado THEN
        INSERT INTO log_estado_solicitud(id_solicitud, estado_anterior, estado_nuevo, fecha_cambio)
        VALUES (
            NEW.id_solicitud,
            OLD.id_estado,
            NEW.id_estado,
            CURRENT_DATE
        );
    END IF;
END $$

DELIMITER ;