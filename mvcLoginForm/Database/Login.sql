Create database UserLogin;
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
create procedure sp_loginPorUsuario
    @_user char(30),
    @_pass char(30)
    as
	select usuario from usuarios where usuario=@_user and contrasena = @_pass
go
create procedure sp_loginPorEmail
    @_email char(75),
    @_pass char(30)
    as
	select usuario from usuarios where email = @_email and contrasena = @_pass
go  
create procedure sp_cambiarContrasena
    @_id int,
    @_pass char(30),
    @_user char(30)
    as
	update usuarios set contrasena = @_pass where id = @_id or usuario = @_user
go  
create procedure sp_verDatosUsuario
    @_user char(30)
    as
	select * from usuarios where usuario = @_user

-- sp_loginPorUsuario 'rodrick', ''