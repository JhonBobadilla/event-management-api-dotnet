# Event Management API (.NET 8, C#)

**Plataforma de Gestión de Eventos**

**Desarrollador:** Jhon Alexander Bobadilla Lombana

---

## 1. Descripción

API RESTful desarrollada en C# con Clean Architecture sobre .NET 8 para la gestión profesional de eventos.
El proyecto implementa la estructura de Clean Architecture y define las entidades principales: User, Event y Attendee.

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

Llena los eventos siguiendo el formato de la plantilla.

Ve a POST /api/Events/upload-excel en Swagger, haz clic en Try it out y adjunta tu archivo Excel usando el campo file.

Haz clic en Execute.

Si el proceso es exitoso, verás un mensaje confirmando cuántos eventos fueron registrados.

Nota: Todos los eventos del archivo serán agregados como si los hubieras registrado uno a uno usando el endpoint manual.

7. Consulta de eventos

Puedes ver todos los eventos usando el endpoint GET /api/Events.

8. Carga de excel.... descarga el formato de excel en la carpeta docs plantilla_eventos.xlsx 

{
  "message": "Archivo recibido y hoja leída correctamente."
}

librería package EPPlus

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




