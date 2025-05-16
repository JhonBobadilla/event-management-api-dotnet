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
    "Key": "************" // Clave de pruebas de los endpoints
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

En la terminal desde la carpeta   EventManagement.Presentation

```bash

dotnet build
dotnet run

```

- La API, documentación y prueba está disponible con Swagger en:

http://localhost:5150/swagger/index.html




{
  "email": "admin@admin.com",
  "password": "admin"
}






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