/*USE master;
GO

-- Cambia 'TuBaseDeDatos' por el nombre real
ALTER DATABASE [UserLogin] 
SET SINGLE_USER 
WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE [UserLogin];*/

Create database UserLogin
go
use UserLogin
go
Create Table usuarios(
	id int primary key identity,
    usuario char(30) unique,
    nombre char(60) not null,
    apellido char(60) not null,
    email char(75) unique,
    contrasena char(30) not null
)
go
insert usuarios values
	('rodrick', 'Rodrigo', 'Castillo', 'rodrick@gmail.com', 'pass123'),
    ('user1', 'Carlos','Perez', 'user1@gmail.com', 'pass321'),
    ('user2', 'Pedro', 'Ramirez', 'user2@gmail.com', 'pass456'),
    ('user3', 'Alberto', 'Gomez', 'user3@gmail.com', 'pass654')
go
create procedure sp_validarLogin
    @_login char(30),
    @_pass char(30)
    as
	select * from usuarios where (usuario=@_login or email=@_login) and contrasena = @_pass
go
create procedure sp_validarContrasena
    @_id int,
    @_user char(30),
    @_pass char(30)
    as
    select contrasena from usuarios where contrasena = @_pass AND (usuario = @_user or id = @_id)
go
create procedure sp_validarUsuario
    @id_actual int = NULL,
    @_user char(30)
    as
    select usuario from usuarios where usuario = @_user AND ( @id_actual IS NULL OR id <> @id_actual)
go
create procedure sp_validarEmail
    @id_actual int = NULL,
    @_email char(75)
    as
    select email from usuarios where email = @_email AND ( @id_actual IS NULL OR id <> @id_actual)
go
create procedure sp_cambiarContrasena
    @_pass char(30),
    @_user char(30)
    as
	update usuarios set contrasena = @_pass where usuario = @_user
go
create procedure sp_crearUsuario
    @_user char(30),
    @_nombre char(60),
    @_apellido char(60),
    @_email char(75),
    @_pass char(30)
    as
    insert usuarios(usuario, nombre, apellido, email, contrasena) values(@_user, @_nombre, @_apellido, @_email, @_pass)
go
create procedure sp_actualizarUsuario
    @_id int,
    @_user char(30),
    @_nombre char(60),
    @_apellido char(60),
    @_email char(75),
    @_pass char(30)
    as
    update usuarios set usuario = @_user, nombre = @_nombre, apellido = @_apellido, email = @_email, contrasena = @_pass where id= @_id

-- sp_validarContrasena 2, '' , '12313'
-- sp_validarUsuario null, 'user1'