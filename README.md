# RestFulApi
Esta aplicación web permite acceder a información de artículos científicos a través de una API RESTful. Los datos se almacenan en una base de datos en línea, y se pueden recuperar mediante solicitudes HTTP.

## Tecnologías utilizadas
* ASP.NET Core
* Entity Framework Core
* Microsoft SQL Server
* C#
* JSON
## Instalación
Para utilizar esta aplicación en tu máquina local, es necesario seguir estos pasos:

Clonar este repositorio en tu máquina local.
Abrir el archivo RestFulApi.sln con Visual Studio.
Restaurar los paquetes NuGet necesarios.
Configurar la conexión a la base de datos en el archivo appsettings.json.
Ejecutar el proyecto desde Visual Studio.
## Uso
## Obtener todos los artículos
Para obtener una lista de todos los artículos almacenados en la base de datos, realiza una solicitud GET a la siguiente URL:

/papers
La respuesta será un objeto JSON que contiene una lista de todos los artículos.

### Obtener un artículo específico
Para obtener información detallada sobre un artículo específico, realiza una solicitud GET a la siguiente URL:

/paper/{id}
Reemplaza {id} con el ID del artículo que deseas recuperar. La respuesta será un objeto JSON que contiene información detallada sobre el artículo.

### Agregar un artículo
Para agregar un nuevo artículo a la base de datos, realiza una solicitud POST a la siguiente URL:

/paper
La solicitud debe incluir un objeto JSON que represente el artículo que deseas agregar.

### Cargar datos
Para cargar datos desde un archivo JSON a la base de datos, realiza una solicitud POST a la siguiente URL:

/papers/loaddata
La solicitud cargará los datos del archivo arxivMetadataOaiSnapshot.json en la base de datos.

Contribución
Si deseas contribuir a este proyecto, puedes hacer lo siguiente:

*Aun no se aceptan contribuciones*

## Licencia
Este proyecto está licenciado bajo la Licencia MIT. Para obtener más información, consulta el archivo LICENSE.md.
