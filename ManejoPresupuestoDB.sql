create database ManejoPresupuesto;
use ManejoPresupuesto;

create table Transacciones
(
Id int identity(1,1) primary key not null,
UsuarioId int not null,
FechaTransaccion datetime not null,
Monto decimal(18, 2) not null,
TipoTransaccionId int not null,
Nota varchar(1000),
CuentaId int not null
);

create table TiposOperaciones
(
Id int identity(1,1) primary key not null,
Descripcion varchar(50) not null
);

create table TiposCuentas
(
Id int identity(1,1) primary key not null,
Nombre varchar(50) not null,
UsuarioId int not null,
Orden int not null
);

create table Cuentas
(
Id int identity(1,1) primary key not null,
Nombre varchar(50) not null,
TipoCuentaId int not null,
Balance decimal(18,2) not null,
Descripcion varchar(1000),
);

create table Usuarios
(
Id int identity(1,1) primary key,
Email varchar(100) not null,
EmailNormalizado varchar(100) not null,
PasswordHash varchar(200) not null,
);

create table Categorias
(
Id int identity(1,1) primary key not null,
Nombre varchar(100) not null,
UsuarioId int not null,
TipoOperacionId int not null
);

-- relación entre tablas

ALTER TABLE Transacciones ADD CONSTRAINT FK_TiposOperaciones
FOREIGN KEY (TipoTransaccionId) REFERENCES TiposOperaciones(Id);

ALTER TABLE Transacciones ADD CONSTRAINT FK_Transacciones_Cuentas
FOREIGN KEY (CuentaId) REFERENCES Cuentas(Id);

ALTER TABLE Transacciones ADD CONSTRAINT FK_Transacciones_Usuarios
FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id);

ALTER TABLE Cuentas ADD CONSTRAINT FK_Cuentas_TiposCuentas
FOREIGN KEY (TipoCuentaId) REFERENCES TiposCuentas(Id);

ALTER TABLE TiposCuentas ADD CONSTRAINT FK_TiposCuentas_Usuarios
FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id);

ALTER TABLE Categorias ADD CONSTRAINT FK_Categorias_Usuarios
FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id);

ALTER TABLE Categorias ADD CONSTRAINT FK_Categorias_TiposOperaciones
FOREIGN KEY (TipoOperacionId) REFERENCES TiposOperaciones(Id);

-- insertar datos
INSERT INTO Transacciones(UsuarioId, FechaTransaccion, Monto, TipoTransaccionId, Nota)
VALUES('Edier', '2021-11-09', 1500.99, 1, 'Esto es una transacción de prueba');

INSERT INTO Transacciones(UsuarioId, FechaTransaccion, Monto, TipoTransaccionId, Nota)
VALUES('Edier', '2021-11-08', 300, 2, null);

INSERT INTO TiposOperaciones(Descripcion)
VALUES('Ingresos');

INSERT INTO TiposOperaciones(Descripcion)
VALUES('Gastos');

-- mostrar los datos
SELECT * FROM Transacciones;

SELECT * FROM TiposOperaciones;

create proc SP_TiposCuentas_Insertar(
	@Nombre varchar(50),
	@UsuarioId int 
)
AS
	DECLARE @Orden int;

	SELECT @Orden = COALESCE(MAX(Orden), 0) + 1
	FROM TiposCuentas
	WHERE UsuarioId = @UsuarioId;

	INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden)
	VALUES(@Nombre, @UsuarioId, @Orden);

	SELECT SCOPE_IDENTITY();