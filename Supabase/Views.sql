CREATE VIEW "ListasView" AS
SELECT "Lista"."Id" as "ListaId", "Perfil"."Id" as "PerfilId", "Perfil"."NomeCompleto", "Lista"."Endereco", "Lista"."Status", "Lista"."CreatedAt" FROM "Lista"
LEFT JOIN "Perfil" ON "Lista"."PerfilId" = "Perfil"."Id"

CREATE VIEW "OrcamentoView" AS
SELECT "Orcamento"."Id" as "OrcamentoId", "Orcamento"."CreatedAt", "Loja"."Id" as "LojaId", "Loja"."Nome" as "LojaNome", "Orcamento"."ListaId", "Orcamento"."SolicitacaoData", "Orcamento"."SolicitacaoHora", "Orcamento"."Recebido", "Orcamento"."RecebidoData", 
"Orcamento"."EntregaPreco" , "Orcamento"."EntregaPrazo" , "Orcamento"."DescontoNoTotal" , "Orcamento"."OrcamentoAnexo" , "Orcamento"."CodigoLoja", "OrcamentoTotal"."PrecoTotal"
FROM "Orcamento"
LEFT JOIN "Loja" ON "Orcamento"."LojaId" = "Loja"."Id"
JOIN "OrcamentoTotal" ON "Orcamento"."Id" = "OrcamentoTotal"."OrcamentoId"


