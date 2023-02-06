CREATE OR REPLACE VIEW "ListasView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT "Lista"."Id" as "ListaId", "Perfil"."Id" as "PerfilId", "Perfil"."NomeCompleto", "Lista"."Endereco", "Lista"."Status", "Lista"."CreatedAt", "Lista"."SoftDeleted", "Lista"."SoftDeletedAt"
FROM "Lista"
LEFT JOIN "Perfil" ON "Lista"."PerfilId" = "Perfil"."Id"
WHERE "Perfil"."SoftDeleted" = false
ORDER BY "Lista"."CreatedAt" DESC;

CREATE OR REPLACE VIEW "OrcamentoView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT "Orcamento"."Id" as "OrcamentoId", "Orcamento"."CreatedAt", "Loja"."Id" as "LojaId", "Loja"."Nome" as "LojaNome", "Orcamento"."ListaId", 
"Orcamento"."SolicitacaoData", "Orcamento"."SolicitacaoHora", "Orcamento"."Recebido", "Orcamento"."RecebidoData", "Orcamento"."EntregaPreco", 
"Orcamento"."EntregaPrazo" , "Orcamento"."DescontoNoTotal" , "Orcamento"."OrcamentoAnexo" , "Orcamento"."CodigoLoja", 
"OrcamentoTotal"."PrecoTotal" as "PrecoTotalSemEntrega", ("Orcamento"."EntregaPreco" + "OrcamentoTotal"."PrecoTotal" )  as "PrecoTotalComEntrega", 
"OrcamentoTotal"."QuantidadeItens", "Orcamento"."SoftDeleted", "Orcamento"."SoftDeletedAt"
FROM "Orcamento"
LEFT JOIN "Loja" ON "Orcamento"."LojaId" = "Loja"."Id"
LEFT JOIN "OrcamentoTotal" ON "Orcamento"."Id" = "OrcamentoTotal"."OrcamentoId"
WHERE "Loja"."SoftDeleted" = false
ORDER BY "PrecoTotalComEntrega" ASC;

CREATE OR REPLACE VIEW "OrcamentoTotal"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT "OrcamentoItem"."OrcamentoId", sum("OrcamentoItem"."Preco" * "OrcamentoItem"."Quantidade") as "PrecoTotal", count("OrcamentoItem"."OrcamentoId") as "QuantidadeItens" 
FROM "OrcamentoItem" 
GROUP BY "OrcamentoItem"."OrcamentoId";


CREATE OR REPLACE VIEW "CarrinhoView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT 
    "Perfil"."NomeCompleto", 
    "Lista"."Endereco", "Lista"."CreatedAt" as "ListaCreatedAt",
    "OrcamentoView"."LojaNome",
    "Carrinho"."Status",
    "Carrinho"."SoftDeleted",
    "Carrinho"."SoftDeletedAt",
    "Carrinho"."CreatedAt"
FROM "Carrinho"
JOIN "Perfil" ON "Carrinho"."PerfilId" = "Perfil"."Id"
JOIN "Lista" on "Carrinho"."ListaId" = "Lista"."Id"
JOIN "OrcamentoView" ON "Carrinho"."OrcamentoId" = "OrcamentoView"."OrcamentoId"




CREATE OR REPLACE VIEW "CarrinhoItemView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT
    "OrcamentoItem"."Descricao",
    "OrcamentoItem"."UnidadeMedida",
    "OrcamentoItem"."Observacao" AS "OrcamentoItem_Observacao",
    "OrcamentoItem"."Preco",
    "OrcamentoItem"."Desconto",
    "CarrinhoItem"."Quantidade",
    "CarrinhoItem"."Observacao" AS "CarrinhoItem_Observacao"
FROM "CarrinhoItem"
JOIN "OrcamentoItem" ON "CarrinhoItem"."OrcamentoItemId" = "OrcamentoItem"."Id"
WHERE "CarrinhoItem"."SoftDeleted" = false
ORDER BY "CarrinhoItem"."CreatedAt" ASC;
