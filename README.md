# RestFulApi
Esta aplicación web permite acceder a información de artículos científicos a través de una API RESTful. Los datos se almacenan en una base de datos en línea, y se pueden recuperar mediante solicitudes HTTP, los datos vienen de un dataset de [Kaggle](https://www.kaggle.com/ "Kaggle"){:target="_blank"}.

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

*Ya viene integrado Swagger por lo que se pueden probar los endpints desde ahí y especifica el formato de envio y respuesta*

## Métodos de los controladores
### Articulos
#### Obtener todos los artículos (20 en 20)
Para obtener una lista de 20 artículos almacenados en la base de datos, realiza una solicitud GET a la siguiente URL:
- **GET/papers?page=1**
- La respuesta será un objeto JSON que contiene una lista de 20 artículos, puedes cambiar el valor de la pagina (Por defecto viene en 1) para recuperar los siguientes 20 articulos.
<br>**_Modelo_**
```
{
  "links": {
    "previousPageLink": "string",
    "nextPageLink": "string"
  },
  "articles": [
    {
      "id": "string",
      "title": "string",
      "abstract": "string",
      "authors": [
        "string"
      ],
      "categories": [
        "string"
      ],
      "publicationDate": "string"
    }
  ]
}
```

#### Obtener un artículo específico
Para obtener información detallada sobre un artículo específico, realiza una solicitud GET a la siguiente URL:
- **GET/papers/{id}**
- Reemplaza {id} con el ID del artículo que deseas recuperar. La respuesta será un objeto JSON que contiene información detallada sobre el artículo.
<br>**_Modelo_**
```
{
  "id": "string",
  "title": "string",
  "abstract": "string",
  "authors": [
    "string"
  ],
  "categories": [
    "string"
  ],
  "publicationDate": "string"
}
```

#### Agregar un artículo
Para agregar un nuevo artículo a la base de datos, realiza una solicitud POST a la siguiente URL:
- **POST/papers**
- La solicitud debe incluir un objeto JSON que represente el artículo que deseas agregar.
<br>**_Modelo_**
```
// En swagger el schema pide otros datos, pero no son necesarios, sigo realizando pruebas por si es necesario ajustarlo
{
  "id": "string",
  "submitter": "string",
  "authors": "string",
  "title": "string",
  "comments": "string",
  "journal-ref": "string",
  "doi": "string",
  "report-no": "string",
  "categories": "string",
  "license": "string",
  "abstract": "string",
  "versions": [
    {
      "id": "string",
      "version": "string",
      "created": "string"
    }
  ],
  "update_date": "string"
}
```

#### Cargar todos los articulos
Para cargar datos desde un archivo JSON a la base de datos, realiza una solicitud POST a la siguiente URL:
- **POST/papers/loaddata**
- La solicitud cargará los datos del archivo arxivMetadataOaiSnapshot.json en la base de datos (Está limitado a cargar solo 100 articulos y no recibe parametros).
<br>**_El archivo puede ser obtenido mediante este link de [Kaggle](https://www.kaggle.com/datasets/Cornell-University/arxiv "Kaggle"){:target="_blank"}_**

### Autores
#### Obtener todos los autores (20 en 20)
Para obtener una lista de 20 autores almacenados en la base de datos, realiza una solicitud GET a la siguiente URL:
- **GET/authors?page=1**
- La respuesta será un objeto JSON que contiene una lista de 20 autores, puedes cambiar el valor de la pagina (Por defecto vieve en 1) para recuperar los siguientes 20 autores.
<br>**_Modelo_**
```
{
  "links": {
    "previousPageLink": "string",
    "nextPageLink": "string"
  },
  "authors": [
    {
      "id": "string",
      "name": "string"
    }
  ]
}
```

#### Obtener un autor específico
Para obtener información detallada sobre un autor específico, realiza una solicitud GET a la siguiente URL:
- **GET/authors/{id}**
- Reemplaza {id} con el ID del autor que deseas recuperar. La respuesta será un objeto JSON que contiene información detallada sobre el autor.
<br>**_Modelo_**
```
{
  "id": "string",
  "name": "string",
  "articles": [
    "string"
  ]
}
```

#### Agregar un autores
Para agregar un nuevo artículo a la base de datos, realiza una solicitud POST a la siguiente URL:
- **POST/authors**
- La solicitud debe incluir un objeto JSON que represente el autor que deseas agregar.
<br>**_Modelo_**
```
// En swagger el schema pide otros datos, pero no son necesarios, sigo realizando pruebas por si es necesario ajustarlo
{
  "id": "string",
  "authorName": "string"
}
```

### Categorias
#### Obtener todos las categorias (20 en 20)
Para obtener una lista de 20 categorias almacenadas en la base de datos, realiza una solicitud GET a la siguiente URL:
- **GET/categories?page=1**
- La respuesta será un objeto JSON que contiene una lista de 20 categorias, puedes cambiar el valor de la pagina (Por defecto viene en 1) para recuperar las siguientes 20.
<br>**_Modelo_**
```
{
  "links": {
    "previousPageLink": "string",
    "nextPageLink": "string"
  },
  "categories": [
    {
      "id": "string",
      "categoryName": "string"
    }
  ]
}
```

#### Obtener una categoria específico
Para obtener información detallada sobre una categoria específica, realiza una solicitud GET a la siguiente URL:
- **GET/categories/{id}**
- Reemplaza {id} con el ID de la categoria que deseas recuperar. La respuesta será un objeto JSON que contiene información detallada sobre la categoria.
<br>**_Modelo_**
```
{
  "id": "string",
  "categoryName": "string",
  "articles": [
    "string"
  ]
}
```

#### Agregar una categoria
Para agregar una nueva categoria a la base de datos, realiza una solicitud POST a la siguiente URL:
- **POST/categories**
- La solicitud debe incluir un objeto JSON que represente la categoria que deseas agregar.
<br>**_Modelo_**
```
// En swagger el schema pide otros datos, pero no son necesarios, sigo realizando pruebas por si es necesario ajustarlo
{
  "id": "string",
  "categoryName": "string"
}
```

## Contribución
Si deseas contribuir a este proyecto, puedes hacer lo siguiente:
<br>**_Aun no se aceptan contribuciones_**

## Licencia
Este proyecto está licenciado bajo la Licencia MIT. Para obtener más información, consulta el archivo LICENSE.txt.
