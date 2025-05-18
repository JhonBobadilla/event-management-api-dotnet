-- Usuarios de ejemplo
INSERT INTO "User" ("FirstName", "LastName", "Email", "PasswordHash", "Phone", "City")
VALUES
  ('Jhon', 'Bobadilla', 'jhonprueba@correo.com', '123456', '3200000000', 'Bogotá'),
  ('Camila', 'López', 'camila@correo.com', 'abcdef', '3112223344', 'Medellín');

-- Eventos de ejemplo
INSERT INTO "Event" ("Title", "Description", "Date", "Location", "Address", "City", "Latitude", "Longitude", "CreatedBy")
VALUES
  ('Conferencia Internacional', 'Un evento técnico para la comunidad.', '2025-06-25 09:00:00', 'Auditorio Principal', 'Calle 123 # 45-67', 'Bogotá', 4.60971, -74.08175, 1),
  ('Feria Tecnológica', 'Evento anual de tecnología.', '2025-07-10 08:00:00', 'Plaza Mayor', 'Cra 15 # 100-50', 'Medellín', 6.24420, -75.58121, 2);

-- Asistentes de ejemplo
INSERT INTO "Attendee" ("FirstName", "LastName", "Email", "Phone", "City", "Address", "EventId", "UserId")
VALUES
  ('Joseph', 'Borda', 'joseph@correo.com', '3207512548', 'Bogotá', 'Calle 123 #45-67', 1, 1),
  ('Luis', 'Torres', 'luis@correo.com', '3105559999', 'Medellín', 'Calle 51 #60-70', 2, NULL);
