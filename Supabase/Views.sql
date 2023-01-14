CREATE VIEW "ListasView" AS
SELECT "Lista"."Id" as "ListaId", "Perfil"."Id" as "PerfilId", "Perfil"."NomeCompleto", "Lista"."Endereco", "Lista"."Status", "Lista"."CreatedAt", "Lista"."SoftDelete", "Lista"."SoftDeletedAt"
FROM "Lista"
LEFT JOIN "Perfil" ON "Lista"."PerfilId" = "Perfil"."Id"

CREATE VIEW "OrcamentoView" AS
SELECT "Orcamento"."Id" as "OrcamentoId", "Orcamento"."CreatedAt", "Loja"."Id" as "LojaId", "Loja"."Nome" as "LojaNome", "Orcamento"."ListaId", "Orcamento"."SolicitacaoData", "Orcamento"."SolicitacaoHora", "Orcamento"."Recebido", "Orcamento"."RecebidoData", 
"Orcamento"."EntregaPreco", "Orcamento"."EntregaPrazo" , "Orcamento"."DescontoNoTotal" , "Orcamento"."OrcamentoAnexo" , "Orcamento"."CodigoLoja", "OrcamentoTotal"."PrecoTotal" as "PrecoTotalSemEntrega", ("Orcamento"."EntregaPreco" + "OrcamentoTotal"."PrecoTotal" )  as "PrecoTotalComEntrega", "OrcamentoTotal"."QuantidadeItens", "Orcamento"."SoftDelete", "Orcamento"."SoftDeletedAt"
FROM "Orcamento"
LEFT JOIN "Loja" ON "Orcamento"."LojaId" = "Loja"."Id"
JOIN "OrcamentoTotal" ON "Orcamento"."Id" = "OrcamentoTotal"."OrcamentoId"
ORDER BY "PrecoTotalComEntrega" ASC

CREATE VIEW "OrcamentoTotal" AS
SELECT "OrcamentoItem"."OrcamentoId", sum("OrcamentoItem"."Preco" * "OrcamentoItem"."Quantidade") as "PrecoTotal", count("OrcamentoItem"."OrcamentoId") as "QuantidadeItens" FROM "OrcamentoItem" GROUP BY "OrcamentoItem"."OrcamentoId"