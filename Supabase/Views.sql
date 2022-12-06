CREATE VIEW ListasView AS
SELECT "Lista".id, "UsuarioPerfil".nomecompleto, "Lista".endereco, "Lista".status, "Lista".created_at FROM "Lista"
LEFT JOIN "UsuarioPerfil" ON "Lista"."UsuarioPerfil_id" = "UsuarioPerfil".id