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
SELECT 
    "Orcamento"."Id" as "OrcamentoId", 
    "Orcamento"."CreatedAt", "Loja"."Id" as "LojaId", 
    "Loja"."Nome" as "LojaNome", 
    "Orcamento"."ListaId", 
    "Orcamento"."SolicitacaoData", 
    "Orcamento"."SolicitacaoHora", 
    "Orcamento"."Recebido", 
    "Orcamento"."RecebidoData", 
    "Orcamento"."EntregaPreco", 
    "Orcamento"."EntregaPrazo", 
    "Orcamento"."DescontoNoTotal", 
    "Orcamento"."OrcamentoAnexo", 
    "Orcamento"."CodigoLoja", 
    COALESCE( "OrcamentoTotal"."PrecoTotal", 0 ) as "PrecoTotalSemEntrega", 
    COALESCE( ("Orcamento"."EntregaPreco" + "OrcamentoTotal"."PrecoTotal" ), 0 ) as "PrecoTotalComEntrega", 
    "OrcamentoTotal"."QuantidadeItens", 
    "Orcamento"."SoftDeleted", 
    "Orcamento"."SoftDeletedAt"
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
    "Perfil"."Id" AS "PerfilId", 
    "Perfil"."NomeCompleto", 
    "Lista"."Id" AS "ListaId", 
    "Lista"."Endereco", 
    "Lista"."CreatedAt" as "ListaCreatedAt",
    "OrcamentoView"."LojaNome",
    "OrcamentoView"."EntregaPrazo",
    "OrcamentoView"."EntregaPreco",
    "Carrinho"."Status",
    "Carrinho"."SoftDeleted",
    "Carrinho"."SoftDeletedAt",
    "Carrinho"."CreatedAt",
    "CarrinhoTotal"."CarrinhoId",
    "CarrinhoTotal"."PrecoTotal",
    "CarrinhoTotal"."QuantidadeItens"
FROM "Carrinho"
JOIN "Perfil" ON "Carrinho"."PerfilId" = "Perfil"."Id"
JOIN "Lista" on "Carrinho"."ListaId" = "Lista"."Id"
JOIN "OrcamentoView" ON "Carrinho"."OrcamentoId" = "OrcamentoView"."OrcamentoId"
JOIN "CarrinhoTotal" ON "CarrinhoTotal"."CarrinhoId" = "Carrinho"."Id"


CREATE OR REPLACE VIEW "CarrinhoTotal"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT
    "CarrinhoItemView"."CarrinhoId",
    sum("CarrinhoItemView"."Preco" * "CarrinhoItemView"."Quantidade") as "PrecoTotal", 
    count("CarrinhoItemView"."CarrinhoItemId") as "QuantidadeItens"
FROM "CarrinhoItemView"
GROUP BY "CarrinhoItemView"."CarrinhoId";




CREATE OR REPLACE VIEW "CarrinhoItemView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT
    "OrcamentoItem"."Id" AS "OrcamentoItemId",
    "OrcamentoItem"."Descricao",
    "OrcamentoItem"."UnidadeMedida",
    "OrcamentoItem"."Observacao" AS "OrcamentoItem_Observacao",
    "OrcamentoItem"."Preco",
    "OrcamentoItem"."Desconto",
    "CarrinhoItem"."Id" AS "CarrinhoItemId",
    "CarrinhoItem"."Quantidade",
    "CarrinhoItem"."Observacao" AS "CarrinhoItem_Observacao",
    "CarrinhoItem"."CarrinhoId"
FROM "CarrinhoItem"
JOIN "OrcamentoItem" ON "CarrinhoItem"."OrcamentoItemId" = "OrcamentoItem"."Id"
WHERE "CarrinhoItem"."SoftDeleted" = false
ORDER BY "CarrinhoItem"."CreatedAt" ASC;


CREATE OR REPLACE VIEW "CarrinhoGroupByListaView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT
	*,
	( COALESCE("CarrinhoGBLView"."PrecoTotal", 0) - COALESCE("CarrinhoGBLView"."OrcamentoMaisCaroPrecoTotalComEntrega", 0) ) AS "Economia"
FROM
	( SELECT
        "CarrinhoView"."ListaId",
        MIN("CarrinhoView"."PerfilId") AS "PerfilId",
        MIN("CarrinhoView"."NomeCompleto") AS "NomeCompleto", 
        MIN("CarrinhoView"."Endereco") AS "Endereco", 
        MIN("CarrinhoView"."ListaCreatedAt") AS "ListaCreatedAt",
        STRING_AGG(DISTINCT("CarrinhoView"."LojaNome"), ', ') AS "Lojas",
        --MIN("CarrinhoView"."LojaNome") AS "Lojas",
        MIN("CarrinhoView"."EntregaPrazo") AS "EntregaPrazoMinimo",
        SUM("CarrinhoView"."EntregaPreco") AS "EntregaPrecoTotal",
        SUM("CarrinhoView"."PrecoTotal") AS "PrecoTotal",
        SUM("CarrinhoView"."QuantidadeItens") AS "QuantidadeItens",
        MAX("OV"."OrcamentoMaisCaroPrecoTotalComEntrega") AS "OrcamentoMaisCaroPrecoTotalComEntrega"
	FROM "CarrinhoView"
	JOIN 
		( SELECT 
			"OrcamentoView"."ListaId",
			MAX("OrcamentoView"."PrecoTotalComEntrega")  AS "OrcamentoMaisCaroPrecoTotalComEntrega"
		FROM "OrcamentoView" 
		GROUP BY "OrcamentoView"."ListaId" ) AS "OV"
		ON "OV"."ListaId" = "CarrinhoView"."ListaId"
	WHERE "CarrinhoView"."Status" LIKE 'Em criação'
	GROUP BY "CarrinhoView"."ListaId" ) AS "CarrinhoGBLView"