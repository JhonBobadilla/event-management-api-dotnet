# Event Management API (.NET 8, C#)

**Plataforma de Gestión de Eventos**

**Desarrollador:** Jhon Alexander Bobadilla Lombana

---

## 1. Descripción

Event Management API es una plataforma backend desarrollada en .NET 8 y C# bajo el patrón Clean Architecture, que permite la gestión profesional de eventos, usuarios y asistentes. Integra autenticación JWT para máxima seguridad y la API de Mapbox para consultar direcciones y lugares de interés cercanos a cada evento. Incluye funciones para carga masiva de eventos por Excel, mediante la API EPPlus, gestión CRUD completa y documentación interactiva mediante Swagger. Ideal para proyectos de eventos, turismo y logística que requieran escalabilidad y robustez. 

---

## 2. Tabla de Contenidos

- [Instalación](#instalación)  
- [Estructura del Proyecto](#estructura-del-proyecto)  
- [Modelo de Datos](#modelo-de-datos)  
- [Modelo Relacional](#modelo-relacional)  
- [Configuración JWT y Seguridad](#configuración-jwt-y-seguridad)  
- [Ejecución y pruebas locales](#ejecución-y-pruebas-locales)  
- [Control de versiones y ramas Git](#control-de-versiones-y-ramas-git)  
- [Hoja de Ruta del Desarrollo](#hoja-de-ruta-del-desarrollo)  
- [Video Explicativo](#video-explicativo) 

---

## 3. Instalación

### Clonar el repositorio

```bash

git clone https://github.com/JhonBobadilla/event-management-api-dotnet.git
cd event-management-api-dotnet
```

Requisitos

- .NET 8 SDK instalado
- PostgreSQL configurado con la base de datos event_management_db
- Editor/IDE recomendado: Visual Studio Code o Visual Studio 2022 


---

## 4. Estructura del Proyecto

/EventManagement.Domain           # Entidades y contratos (C#)
/EventManagement.Application      # Casos de uso y lógica de negocio
/EventManagement.Infrastructure   # Acceso a datos y servicios externos
/EventManagement.Presentation     # API RESTful (.NET 8, C#)

---

## 5. Modelo de Datos

Entidades principales en la capa Domain:

- User: Usuario autenticado y creador de eventos.
- Event: Información de eventos (título, descripción, fecha, ubicación, creador).
- Attendee: Personas registradas como asistentes a los eventos.

---

## 6. Modelo Relacional

![Diagrama Modelo Relacional](docs/modelo-relacional.png)

---

## 7. Configuración JWT y Seguridad

En appsettings.json, configure la clave secreta para JWT:

```csharp
"Jwt": {
    "Key": "************"
  }
```

La API protege sus endpoints mediante autenticación JWT. Solo usuarios con token válido pueden realizar operaciones que modifiquen datos.

La configuración de autenticación está implementada en Program.cs:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
```
---

## 8. Ejecución y pruebas locales

1. Desde la terminal, en la carpeta `EventManagement.Presentation`, ejecuta:

```bash
dotnet build
dotnet run
```
2. La API estará disponible con Swagger en:

http://localhost:5150/swagger/index.html

3. Registro de usuario

Antes de iniciar sesión, registra un usuario usando el endpoint:POST /api/Auth/register
Haz clic en Try it out y envía un cuerpo como este:

```JSON
{
  "firstName": "Jhon",
  "lastName": "Bobadilla",
  "email": "jhonprueba@correo.com",
  "password": "123456",
  "phone": "3200000000",
  "city": "Bogotá"
}
```
- Si la operación es exitosa, verás:

```JSON
{
  "message": "Usuario registrado exitosamente."
}
```
4. AUTENTICACIÓN:

- Dirígete a POST /api/Auth/login → haz clic en Try it out.
- Reemplaza el cuerpo con el correo y contraseña del usuario recién registrado, manteniendo el formato JSON:

```JSON
{
  "email": "jhonprueba@correo.com",
  "password": "123456"
}

```
- Haz clic en Execute y copia el token que se muestra.

5. AUTORIZACIÓN

- Haz clic en el botón Authorize (esquina superior derecha).
- En el prompt, escribe Bearer, (seguido de un espacio) pega el token copiado.

  formato correcto:   Bearer eyJhbGciOi.........4f8uQ7Suo 
  formato incorrecto (Swagger lo pega así): Bearer {"token": "eyJhbGciOi.........4f8uQ7Suo"} 

Borra los caracteres que sobran al inicio: ({"token": ") y al final ("}), hasta dejarlo en el formato correcto.

Recomendación:
Copia el token desde la consola del backend (donde también se imprime limpio).

Con esto podrás autenticarte correctamente y probar todos los endpoints protegidos como GET /api/Events.

---

6. Creación de eventos manualmente y con el excel

- CREACIÓN MANUAL 

- Crea un evento manualmente usando el endpoint: POST /api/Events
  Haz clic en Try it out y envía un cuerpo como este:

```JSON
{
  "title": "Conferencia Internacional",
  "description": "Un evento técnico para la comunidad.",
  "date": "2025-06-25T09:00:00",
  "location": "Auditorio Principal",
  "address": "Calle 123 # 45-67",
  "city": "Bogotá",
  "latitude": 4.60971,
  "longitude": -74.08175
}
```
- CREACIÓN MASIVA MEDIANTE ARCHIVO DE EXCEL  

Descarga la plantilla de Excel "plantilla_eventos.xlsx" que está en la carpeta /docs del repositorio.

Llena los eventos siguiendo el formato exacto de la plantilla.

Ve a POST /api/Events/upload-excel en Swagger, haz clic en Try it out y adjunta tu archivo Excel usando el campo file.

Haz clic en Execute.

Si el proceso es exitoso, verás un mensaje confirmando cuántos eventos fueron registrados.

Nota: Todos los eventos del archivo serán agregados como si los hubieras registrado uno a uno usando el endpoint manual.

7. Consulta de eventos

Puedes ver todos los eventos usando el endpoint GET /api/Events.

8. Registro de Asistentes a Eventos

La API permite registrar asistentes a un evento específico de forma sencilla y flexible. Cada asistente puede ser un usuario registrado o un invitado externo, y la información queda almacenada y asociada al evento correspondiente. 

- Cómo funciona: 

Puedes registrar asistentes a cualquier evento mediante el endpoint: POST /api/Attendees/register

Debes enviar en el cuerpo de la solicitud (body) los datos del asistente y el evento al que se va a inscribir, en formato JSON.

Cuerpo de la petición (ejemplo):

```JSON
{
  "eventId": 17,
  "firstName": "Joseph",
  "lastName": "Borda",
  "email": "joseph@correo.com",
  "phone": "3207512548",
  "city": "Bogotá",
  "address": "Calle 123 #45-67"
}
```
9. Geolocalización: Consulta de Direcciones y Lugares Cercanos

La API integra la funcionalidad de búsqueda de direcciones y lugares de interés cercanos a las cordenadas del evento consultado utilizando el servicio de Mapbox, cada evento almacena coordenadas geográficas (latitude y longitude), que son las que se envian a la Api externa y esta devuelve los lugares y direcciones de interés. 

- Cómo funciona:

Puedes consultar lugares cercanos a un evento mediante el endpoint: GET /api/Events/{id}/nearby, ingresa el id del evento a consultar, el parámetro radius es opcional y define el radio de búsqueda en metros (por defecto 500m, máximo 10,000m).

Cada elemento incluye:

name: Nombre de la dirección o lugar de interés.
type: Puede ser "address" (dirección) o "poi" (lugar de interés).
latitude y longitude: Coordenadas del lugar encontrado.
distance: Distancia en metros desde el evento consultado.

Detalles técnicos

- El servicio usa Mapbox Geocoding API y retorna hasta 10 resultados relevantes (direcciones y lugares de interés), la búsqueda considera ambos tipos (address y poi) para mayor utilidad.

- Nota sobre las coordenadas

Las coordenadas deben ingresarse correctamente como números decimales (latitude, longitude).

Ejemplo para Bogotá centro:
Latitude: 4.60971 - Longitude: -74.08175

- ¿Para qué sirve?

Esta funcionalidad es ideal para:

•	Sugerir hoteles, restaurantes o atracciones cercanas a tus eventos.
•	Integrar mapas y navegación en plataformas de eventos.
•	Proyectos de turismo, logística, reservas y mucho más.

10. 








11. Otros endpoints:

GET /api/Events/{id}:
Consulta los detalles de un evento específico según su ID.

PUT /api/Events/{id}:
Modifica la información de un evento existente. Ingresa el ID, copia toda la información actual del evento (formato JSON, que puedes obtener usando el endpoint GET /api/Events/{id}), realiza los cambios necesarios y haz clic en Execute.

DELETE /api/Events/{id}:
Elimina un evento. Solo debes ingresar el ID del evento que deseas eliminar y hacer clic en Execute.

---































## 9. Control de versiones y ramas Git

El flujo de trabajo usa las siguientes ramas:

- dev: Desarrollo activo con commits frecuentes.
- test: Pruebas e integración.
- main: Versión estable para producción.

Flujo:

dev → test → main

---

## 10. Hoja de Ruta del Desarrollo

Integración de carga y procesamiento masivo de archivos Excel con eventos.

Consulta de ubicaciones cercanas usando API de Mapbox.

Análisis avanzado de asistentes por día de la semana.

Dockerización y despliegue escalable (BONUS).

---

## 11. Video Explicativo

El video privado donde se explica la solución y ejecución del proyecto está disponible en:

[xxxxxxxxxxxxxxxxxxxxxxxxxx]


