CREATE VIEW ListasView AS
SELECT "Lista".id as "ListaId", "UsuarioPerfil"."id" as "UsuarioPerfilId", "UsuarioPerfil".nomecompleto, "Lista".endereco, "Lista".status, "Lista".created_at FROM "Lista"
LEFT JOIN "UsuarioPerfil" ON "Lista"."UsuarioPerfil_id" = "UsuarioPerfil".id

CREATE VIEW OrcamentoView AS
SELECT "Orcamento"."id" as "OrcamentoId", "Orcamento"."created_at", "Loja"."id" as "LojaId", "Loja"."nome", "Orcamento"."ListaId", "Orcamento"."SolicitacaoData", "Orcamento"."Recebido", "Orcamento"."RecebidoData", 
"Orcamento"."EntregaPreco" , "Orcamento"."EntregaPrazo" , "Orcamento"."DescontoNoTotal" , "Orcamento"."OrcamentoAnexo" , "Orcamento"."CodigoLoja" 
FROM "Orcamento"
LEFT JOIN "Loja" ON "Orcamento"."LojaId" = "Loja".id


