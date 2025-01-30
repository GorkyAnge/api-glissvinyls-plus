# GlissVinyls-Plus API

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Status](https://img.shields.io/badge/Status-In%20Development-yellowgreen.svg)

GlissVinyls-Plus es una API diseñada para gestionar stock de almacenes. Permite a los usuarios realizar operaciones de entradas y salidas de productos, crear almacenes, ofrecer recomendaciones personalizadas de adquisición de stock por cada almacén, mostrar historial de movimientos de los almacenes, además de ofrecer un sistema de autenticación que permite el registro, login y logout. Este proyecto ha sido desarrollado como parte de la asignatura de **Ingeniería Web**.
## Documentación

[PROYECTO INTEGRADOR INGENIERÍA WEB (ABAD Y PALACIOS) (1).pdf](https://github.com/user-attachments/files/18605968/PROYECTO.INTEGRADOR.INGENIERIA.WEB.ABAD.Y.PALACIOS.1.pdf)


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

| **Método** | **Endpoint**           | **Descripción**                      |
|------------|------------------------|--------------------------------------|
| POST       | `/api/Auth/register`   | Registro de nuevos usuarios.         |
| POST       | `/api/Auth/login`      | Iniciar sesión.                      |
| POST       | `/api/Auth/logout`     | Cerrar sesión.                       |

### Categorías

| **Método** | **Endpoint**                     | **Descripción**                                 |
|------------|----------------------------------|-------------------------------------------------|
| GET        | `/api/Categories`                | Obtener todas las categorías.                   |
| POST       | `/api/Categories`                | Crear una nueva categoría.                      |
| GET        | `/api/Categories/{id}`           | Obtener una categoría por su ID.                |
| PUT        | `/api/Categories/{id}`           | Actualizar una categoría por su ID.             |
| DELETE     | `/api/Categories/{id}`           | Eliminar una categoría por su ID.               |

### Inventario

| **Método** | **Endpoint**           | **Descripción**                      |
|------------|------------------------|--------------------------------------|
| POST       | `/api/Inventory/acquire` | Adquirir nuevos inventarios de productos. |

### Historial de Movimientos

| **Método** | **Endpoint**                                          | **Descripción**                                   |
|------------|-------------------------------------------------------|---------------------------------------------------|
| GET        | `/api/MovementHistory/ByWarehouse/{warehouseId}`      | Obtener el historial de movimientos por almacén.  |
| GET        | `/api/MovementHistory`                                | Obtener todo el historial de movimientos.         |
| GET        | `/api/MovementHistory/Exit`                           | Obtener el historial de salidas de stock.         |
| GET        | `/api/MovementHistory/Entry`                          | Obtener el historial de entradas de stock.         |
| GET        | `/api/MovementHistory/ByWarehouse/{warehouseId}/Entry` | Obtener el historial de entradas por almacén.      |
| GET        | `/api/MovementHistory/ByWarehouse/{warehouseId}/Exit`  | Obtener el historial de salidas por almacén.       |

### Productos

| **Método** | **Endpoint**               | **Descripción**                                           |
|------------|----------------------------|-----------------------------------------------------------|
| GET        | `/api/Products`            | Obtener todos los productos.                              |
| POST       | `/api/Products`            | Crear un nuevo producto (requiere autenticación).        |
| GET        | `/api/Products/{id}`       | Obtener un producto por su ID.                            |
| PUT        | `/api/Products/{id}`       | Actualizar un producto por su ID (requiere autenticación).|
| DELETE     | `/api/Products/{id}`       | Eliminar un producto por su ID (requiere autenticación). |

### Recomendaciones

| **Método** | **Endpoint**                                               | **Descripción**                                                      |
|------------|------------------------------------------------------------|----------------------------------------------------------------------|
| GET        | `/api/Recommendations/top-selling-products`               | Obtener los productos más vendidos.                                  |
| GET        | `/api/Recommendations/predict-stock-needs`                | Predecir las necesidades de stock.                                   |
| GET        | `/api/Recommendations/top-selling-products-by-warehouse/{warehouseId}` | Obtener los productos más vendidos por almacén.                      |
| GET        | `/api/Recommendations/predict-stock-needs-by-warehouse/{warehouseId}`  | Predecir las necesidades de stock por almacén.                        |

### Ventas

| **Método** | **Endpoint**                  | **Descripción**                      |
|------------|-------------------------------|--------------------------------------|
| POST       | `/api/Sales/register-sale`    | Registrar una nueva venta.           |

### Stocks

| **Método** | **Endpoint**                             | **Descripción**                                   |
|------------|------------------------------------------|---------------------------------------------------|
| GET        | `/api/Stocks`                            | Obtener todos los stocks.                         |
| POST       | `/api/Stocks`                            | Crear un nuevo stock.                             |
| GET        | `/api/Stocks/{id}`                       | Obtener un stock por su ID.                       |
| PUT        | `/api/Stocks/{id}`                       | Actualizar un stock por su ID.                    |
| DELETE     | `/api/Stocks/{id}`                       | Eliminar un stock por su ID.                      |
| GET        | `/api/Stocks/warehouse/{warehouseId}`     | Obtener los stocks por almacén.                   |
| PUT        | `/api/Stocks/{id}/quantity`              | Actualizar la cantidad de un stock específico.    |

### Proveedores

| **Método** | **Endpoint**               | **Descripción**                                           |
|------------|----------------------------|-----------------------------------------------------------|
| GET        | `/api/Suppliers`           | Obtener todos los proveedores.                            |
| POST       | `/api/Suppliers`           | Crear un nuevo proveedor.                                 |
| GET        | `/api/Suppliers/{id}`      | Obtener un proveedor por su ID.                           |
| PUT        | `/api/Suppliers/{id}`      | Actualizar un proveedor por su ID.                        |
| DELETE     | `/api/Suppliers/{id}`      | Eliminar un proveedor por su ID.                          |

### Almacenes (Warehouses)

| **Método** | **Endpoint**                     | **Descripción**                                           |
|------------|----------------------------------|-----------------------------------------------------------|
| GET        | `/api/Warehouses`                | Obtener todos los almacenes.                              |
| POST       | `/api/Warehouses`                | Crear un nuevo almacén.                                   |
| GET        | `/api/Warehouses/{id}`           | Obtener un almacén por su ID.                             |
| PUT        | `/api/Warehouses/{id}`           | Actualizar un almacén por su ID.                          |
| DELETE     | `/api/Warehouses/{id}`           | Eliminar un almacén por su ID.                            |

### Documentación de la API

Puedes acceder a la documentación completa de la API en formato OpenAPI (OAS3) a través del siguiente enlace:

- [Swagger JSON](https://glissvinyls-plus-web-api.azurewebsites.net/swagger/v1/swagger.json)

---


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
[LinkedIn]([https://linkedin.com/in/usuario](https://www.linkedin.com/in/gorky-palacios-mutis-8136ab230/))
