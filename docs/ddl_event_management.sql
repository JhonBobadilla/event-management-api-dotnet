-- Tabla de usuarios
CREATE TABLE "User" (
    "Id" SERIAL PRIMARY KEY,
    "FirstName" VARCHAR(50) NOT NULL,
    "LastName" VARCHAR(50) NOT NULL,
    "Email" VARCHAR(100) UNIQUE NOT NULL,
    "PasswordHash" VARCHAR(200) NOT NULL,
    "Phone" VARCHAR(20),
    "City" VARCHAR(100),
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de eventos
CREATE TABLE "Event" (
    "Id" SERIAL PRIMARY KEY,
    "Title" VARCHAR(150) NOT NULL,
    "Description" VARCHAR(500),
    "Date" TIMESTAMP NOT NULL,
    "Location" VARCHAR(150),
    "Address" VARCHAR(200),
    "City" VARCHAR(100),
    "Latitude" FLOAT,
    "Longitude" FLOAT,
    "CreatedBy" INTEGER NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de asistentes
CREATE TABLE "Attendee" (
    "Id" SERIAL PRIMARY KEY,
    "FirstName" VARCHAR(50) NOT NULL,
    "LastName" VARCHAR(50) NOT NULL,
    "Email" VARCHAR(100) NOT NULL,
    "Phone" VARCHAR(20),
    "City" VARCHAR(100),
    "Address" VARCHAR(200),
    "RegisteredAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "EventId" INTEGER NOT NULL REFERENCES "Event"("Id") ON DELETE CASCADE,
    "UserId" INTEGER REFERENCES "User"("Id")
);
