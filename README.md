# Task Manager Service API

**Descripcion**

Es una API de tipo RESTfull encargada principalmente de la operaciones CRUD a una base de datos SQL Server la cual esta pensada para ser consumida por una aplicacion cliente
La cual se encarga de mostrar de forma atractiva los datos que la API le proporciona mediante las consultas HTTPS 

**Funcionalidades**

Autentificacion: Se integro el uso de JWT para realizar la validacion de usuarios y mantener una mayor seguridad en los datos

Crear tareas: Permite a los usuarios añadir nuevas tareas especificando detalles como el título, la descripcion, la fecha de vencimiento, la prioridad, el estado y el usuario asignado

Leer tareas: Proporciona una vista general de todas las tareas existentes, con detalles completos de cada una devolviendo unicamente las tareas segun el token generado cuando un usuario se autentifica

Actualizar tareas: Permite modificar los detalles de las tareas existentes

Eliminar tareas: Facilita la eliminación de tareas que ya no son necesarias

Registrar: Permite relizar el registro de nuevos usuarios

Actualizar Usuario: Proporciona la funcionalidad de que los usuario puedan modificar sus credenciales de inicio de sesion o informacion personal

**Tecnologías Utilizadas**

ASP.NET Core: Framework utilizado para desarrollar la API REST

Entity Framework Core: ORM utilizado para interactuar con la base de datos

**Requisitos**

.NET 8.0 o superior
SQL Server (o cualquier otro proveedor de base de datos compatible con Entity Framework Core)

# Estructura del Proyecto

Controllers: Contiene los controladores para gestionar las solicitudes HTTP y realizar las operaciones CRUD

Context: Contiene las clases de entidad que representan las tablas de la base de datos

RequestModel: Contiene la clase modelo a utilizar cuando un usuario desea hacer un cambio de contraseña

# Configuracion de la base de datos

appsettings.json: Aqui encontrara la cadena de conexion a la base de datos para establecer una conexion de manera LocalHost o Remota
TaskManagerDB.Sql: Script a ejecutar para inicializar la base de datos
