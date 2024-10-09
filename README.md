# GlissVinyls-Plus API

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Status](https://img.shields.io/badge/Status-In%20Development-yellowgreen.svg)

GlissVinyls-Plus es una API diseñada para gestionar un sistema de ecommerce de vinilos. Permite a los usuarios realizar operaciones CRUD sobre los productos, además de ofrecer un sistema de autenticación que permite el registro, login y logout. Este proyecto ha sido desarrollado como parte de la asignatura de **Ingeniería Web**.

## Tabla de Contenidos
- [Instalación](#instalación)
- [Uso](#uso)
- [Características](#características)
- [Endpoints](#endpoints)
- [Contribuciones](#contribuciones)
- [Licencia](#licencia)
- [Autor](#autor)

## Instalación

Sigue estos pasos para clonar y ejecutar el proyecto en tu máquina local:

1. Clona el repositorio:
   ```bash
   git clone https://github.com/usuario/glissvinyls-plus.git
   ```
2. Navega al directorio del proyecto:
   ```bash
   cd glissvinyls-plus
   ```
3. Restaura las dependencias del proyecto:
```bash
dotnet restore
```
4. Inicia el servidor localmente:
```bash
dotnet run
```
## Uso

### Requisitos previos:
- **.NET 8** instalado en tu sistema.
- **SQL Server** o **SQL Express** como base de datos.

Una vez el servidor esté ejecutándose, podrás acceder a los endpoints de la API utilizando herramientas como Postman o cURL.

## Características
- **CRUD de Productos**: Crea, lee, actualiza y elimina productos en el sistema.
- **Autenticación**: Sistema de registro, login y logout con manejo de JWT.
- **Autorización basada en roles** (en desarrollo): Control de acceso para operaciones sensibles como la creación y edición de productos.

## Endpoints

### Autenticación
| Método | Endpoint        | Descripción                |
|--------|-----------------|----------------------------|
| POST   | `/api/Auth/register` | Registro de nuevos usuarios. |
| POST   | `/api/Auth/login`    | Iniciar sesión.              |
| POST   | `/api/Auth/logout`   | Cerrar sesión.               |

### Productos
| Método | Endpoint        | Descripción                                  |
|--------|-----------------|----------------------------------------------|
| GET    | `/api/products` | Obtener todos los productos.                 |
| GET    | `/api/products/{id}` | Obtener un producto por su ID.            |
| POST   | `/api/products` | Crear un nuevo producto (requiere autenticación). |
| PUT    | `/api/products/{id}` | Actualizar un producto por su ID (requiere autenticación). |
| DELETE | `/api/products/{id}` | Eliminar un producto por su ID (requiere autenticación). |

## Contribuciones

Las contribuciones son bienvenidas. Si deseas colaborar, por favor sigue estos pasos:
1. Haz un fork del proyecto.
2. Crea una nueva rama para tu funcionalidad (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz un commit (`git commit -m 'Añadir nueva funcionalidad'`).
4. Haz un push a la rama (`git push origin feature/nueva-funcionalidad`).
5. Abre un pull request.

## Licencia

Este proyecto está licenciado bajo la [MIT License](https://opensource.org/licenses/MIT).

## Autor

**Gorky Palacios Mutis**  
Estudiante de Ingeniería de Software  
[LinkedIn](https://linkedin.com/in/usuario)
