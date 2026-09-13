-- BaseDatos.sql
-- Esquema + datos semilla para los dos microservicios:
--   clientedb -> ClienteService (Persona, Cliente)
--   cuentadb  -> CuentaService  (Cuenta, Movimiento)
--
-- Pensado para correr una sola vez contra un Postgres recien levantado
-- (se monta como script de init del contenedor oficial en docker-compose.yml).

-- ============================================================
-- clientedb
-- ============================================================
CREATE DATABASE clientedb;

\connect clientedb

-- Persona/Cliente se modelan como herencia real en el dominio (Cliente : Persona),
-- mapeada con Table-Per-Hierarchy en EF Core: todo va a una sola tabla "clientes".
-- "id" es la PK tecnica heredada de Persona; "cliente_id" es la clave de login de Cliente.
CREATE TABLE clientes (
    id              SERIAL PRIMARY KEY,
    nombre          VARCHAR(150)    NOT NULL,
    genero          VARCHAR(20)     NOT NULL,
    edad            SMALLINT        NOT NULL CHECK (edad >= 0),
    identificacion  VARCHAR(20)     NOT NULL UNIQUE,
    direccion       VARCHAR(250)    NOT NULL,
    telefono        VARCHAR(20)     NOT NULL,
    cliente_id      VARCHAR(20)     NOT NULL UNIQUE,
    contrasena      VARCHAR(250)    NOT NULL,
    estado          BOOLEAN         NOT NULL DEFAULT TRUE
);

INSERT INTO clientes (nombre, genero, edad, identificacion, direccion, telefono, cliente_id, contrasena, estado) VALUES
    ('Jose Lema',            'Masculino', 35, '1000000001', 'Otavalo sn y principal',      '098254785', 'jlema',     '1234', TRUE),
    ('Marianela Montalvo',   'Femenino',  40, '1000000002', 'Amazonas y NNUU',             '097548965', 'mmontalvo', '5678', TRUE),
    ('Juan Osorio',          'Masculino', 28, '1000000003', '13 junio y Equinoccial',      '098874587', 'josorio',   '1245', TRUE);


-- ============================================================
-- cuentadb
-- ============================================================
CREATE DATABASE cuentadb;

\connect cuentadb

-- Sin FK fisica hacia clientes: es otro microservicio, otra base. La relacion se
-- resuelve por cliente_identificacion (llega via el evento async, nunca por JOIN).
CREATE TABLE cuentas (
    cuenta_id               SERIAL PRIMARY KEY,
    numero_cuenta           VARCHAR(20)     NOT NULL UNIQUE,
    tipo_cuenta             VARCHAR(20)     NOT NULL, -- Ahorros | Corriente
    saldo_inicial           NUMERIC(18,2)   NOT NULL DEFAULT 0, -- historico, no cambia
    saldo                   NUMERIC(18,2)   NOT NULL DEFAULT 0, -- disponible actual
    estado                  BOOLEAN         NOT NULL DEFAULT TRUE,
    cliente_identificacion  VARCHAR(20)     NOT NULL
);

CREATE TABLE movimientos (
    movimiento_id   SERIAL PRIMARY KEY,
    cuenta_id       INTEGER         NOT NULL REFERENCES cuentas(cuenta_id) ON DELETE CASCADE,
    fecha           TIMESTAMPTZ     NOT NULL DEFAULT now(),
    tipo_movimiento VARCHAR(20)     NOT NULL, -- Deposito | Retiro
    valor           NUMERIC(18,2)   NOT NULL,
    saldo           NUMERIC(18,2)   NOT NULL
);

CREATE INDEX ix_movimientos_cuenta_fecha ON movimientos (cuenta_id, fecha);

-- "saldo" ya incluye el efecto de los movimientos de mas abajo.
INSERT INTO cuentas (numero_cuenta, tipo_cuenta, saldo_inicial, saldo, estado, cliente_identificacion) VALUES
    ('478758', 'Ahorros',   2000.00, 1425.00, TRUE, '1000000001'), -- Jose Lema, retiro de 575
    ('225487', 'Corriente', 100.00,  700.00,  TRUE, '1000000002'), -- Marianela, deposito de 600
    ('495878', 'Ahorros',   0.00,    150.00,  TRUE, '1000000003'), -- Juan, deposito de 150
    ('496825', 'Ahorros',   540.00,  0.00,    TRUE, '1000000002'), -- Marianela, retiro de 540
    ('585545', 'Corriente', 1000.00, 1000.00, TRUE, '1000000001'); -- Jose, cuenta nueva sin movimientos

INSERT INTO movimientos (cuenta_id, fecha, tipo_movimiento, valor, saldo) VALUES
    ((SELECT cuenta_id FROM cuentas WHERE numero_cuenta = '478758'), '2022-02-08', 'Retiro',   -575.00, 1425.00),
    ((SELECT cuenta_id FROM cuentas WHERE numero_cuenta = '225487'), '2022-02-10', 'Deposito',  600.00,  700.00),
    ((SELECT cuenta_id FROM cuentas WHERE numero_cuenta = '495878'), '2022-02-08', 'Deposito',  150.00,  150.00),
    ((SELECT cuenta_id FROM cuentas WHERE numero_cuenta = '496825'), '2022-02-08', 'Retiro',   -540.00,    0.00);

-- Cache de nombre/estado de cliente que en producto se llena sola via el consumer de
-- RabbitMQ (ClienteActualizadoEvent). Se siembra aca para que el reporte funcione
-- desde el primer arranque, sin depender de que ya haya pasado al menos un mensaje.
CREATE TABLE clientes_info (
    cliente_identificacion  VARCHAR(20) PRIMARY KEY,
    nombre                  VARCHAR(150) NOT NULL,
    estado                  BOOLEAN NOT NULL DEFAULT TRUE
);

INSERT INTO clientes_info (cliente_identificacion, nombre, estado) VALUES
    ('1000000001', 'Jose Lema',          TRUE),
    ('1000000002', 'Marianela Montalvo', TRUE),
    ('1000000003', 'Juan Osorio',        TRUE);
