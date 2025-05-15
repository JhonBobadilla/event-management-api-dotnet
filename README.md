# Event Management API (.NET 8, C#)

**Plataforma de Gestión de Eventos**

Desarrollador: Jhon Alexander Bobadilla Lombana

---

## Descripción

API RESTful desarrollada en **C#** con Clean Architecture sobre **.NET 8** para la gestión profesional de eventos.  
El proyecto implementa la estructura de Clean Architecture y define las entidades principales: User, Event y Attendee.

---

## Tabla de Contenidos

- [Instalación](#instalación)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Modelo de Datos](#modelo-de-datos)
- [Hoja de Ruta del Desarrollo](#hoja-de-ruta-del-desarrollo)
- [Video Explicativo](#video-explicativo)

---

## Instalación

1. **Clonar el repositorio**
   
   git clone https://github.com/JhonBobadilla/event-management-api-dotnet.git
   cd event-management-api-dotnet

2. **Requisitos**

.NET 8 SDK
PostgresSQL (configurado con la base de datos event_management_db)
Editor/IDE recomendado: Visual Studio Code o Visual Studio 2022

3. **Estructura del Proyecto**

/EventManagement.Domain          # Entidades y contratos (C#)
/EventManagement.Application     # Casos de uso y lógica de negocio
/EventManagement.Infrastructure  # Acceso a datos y servicios externos
/EventManagement.Presentation    # API RESTful (.NET 8, C#)

4. **Modelo de Datos**

El proyecto incluye las siguientes entidades principales en la capa Domain:

User: Usuario autenticado, creador de eventos.
Event: Información de eventos (título, descripción, fecha, ubicación, creador).
Attendee: Personas registradas como asistentes a los eventos.

5. **Modelo Relacional**

El siguiente diagrama muestra el modelo entidad-relación de la base de datos utilizado en el proyecto.

![Modelo Relacional](docs/modelo-relacional.png)

