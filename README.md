# Validaciones
Las validaciones de campo son de usuario, email, contraseña, y el repetir contraseña.

Ir cambiando los datos de ejemplos de abajo para ir probando
# DOMINIO
Al ejecutar el codigo se cargara la vista principal

Copiar el dominio localhost y al final agregar /api/Auth/[accion]



# Probar login
    {
      "login" : "rodrick",
      "contrasena": "pass123"
    }
Metodo POST: https://localhost:44305/api/Auth/login

# Probar actualizar
    {
      "usuario" : "rodrick",
      "nombre" : "Alessandro",
      "apellido" : "Castillo",
      "email" : "rodrick@gmail.com",
      "contrasena" : "123",
      "r_contrasena" : "123"
    }
Metodo PUT: https://localhost:44305/api/Auth/actualizar --> Agregar Token el cual te lo da el login

La autenticacion es Bearer Token -> Pegar sin las " "

## Ejemplo
<img width="723" height="463" alt="image" src="https://github.com/user-attachments/assets/4a47f78e-090d-4ca1-bc1a-6402c5200d97" />


# Probar registrar
    {
      "usuario" : "casti",
      "nombre" : "Alessandro",
      "apellido" : "Castillo",
      "email" : "casti@gmail.com",
      "contrasena" : "123",
      "r_contrasena" : "123"
    }
Metodo POST: https://localhost:44305/api/Auth/registrar

# Probar recuperar
    {
      "usuario" : "rodrick",
      "contrasena" : "1234",
      "r_contrasena" : "1234"
    }
Metodo PATCH: https://localhost:44305/api/Auth/recuperar
