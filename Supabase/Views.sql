CREATE OR REPLACE VIEW "ListasView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT 
    "Lista"."Id" as "ListaId", 
    "Perfil"."Id" as "PerfilId", 
    "Perfil"."NomeCompleto", 
    "Lista"."Endereco", 
    "Lista"."Status", 
    "Lista"."CreatedAt", 
    "Lista"."SoftDeleted", 
    "Lista"."SoftDeletedAt",
    "EconomiaOrcamentoItemTotalView"."PrecoTotal" AS "EconomiaPrecoTotalSemEntrega",
    "EconomiaOrcamentosEntregaTotalView"."EntregaPrecoTotal",
    ( "EconomiaOrcamentoItemTotalView"."PrecoTotal" + "EconomiaOrcamentosEntregaTotalView"."EntregaPrecoTotal" ) as "EconomiaPrecoTotalComEntrega"
FROM 
    "Lista"
    LEFT JOIN "Perfil" ON "Lista"."PerfilId" = "Perfil"."Id"
    LEFT JOIN "EconomiaOrcamentoItemTotalView" ON "EconomiaOrcamentoItemTotalView"."ListaId"  = "Lista"."Id"
    LEFT JOIN "EconomiaOrcamentosEntregaTotalView" on "EconomiaOrcamentosEntregaTotalView"."ListaId"  = "Lista"."Id"
WHERE 
    "Perfil"."SoftDeleted" = false
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


-- //TODO daria pra criar essa view usando OrcamentoItemView. quem sabe fosse mais otimizado.
CREATE OR REPLACE VIEW "OrcamentoTotal"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT 
    "OrcamentoItem"."OrcamentoId",
    sum("OrcamentoItem"."Preco" * coalesce("OrcamentoItem"."Quantidade", (select li."Quantidade"  
                                                                            from "ListaItem" li 
                                                                            where li."Id" = "OrcamentoItem"."ListaItemId") , 1) ) as "PrecoTotal", 
    count("OrcamentoItem"."OrcamentoId") as "QuantidadeItens" 
FROM "OrcamentoItem" 
WHERE "OrcamentoItem"."SoftDeleted" = false 
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


-- // TODO há um problema nessa view que ela nao está apresentando os valores totalmente certos.
-- quando há dois itens de orçamentos diferentes com o mesmo menor valor, as vezes
-- ela acaba retornar nao o item ideal, que seria o item da mesma loja q as outras, ou seja, mais otimizado.
--  oq faz com que o vlaor total fique mais caro pq acaba somando o frente de duas lojas qdo podia ser de só uma.
CREATE OR REPLACE VIEW "EconomiaOrcamentoItemView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT
	( 	SELECT 
            "ListaItem"."ListaId" 
		FROM "ListaItem" 
		WHERE 
			"ListaItem"."Id" = "QueryMIN"."ListaItemId"  
		LIMIT 1 ) AS "ListaId",
    "QueryMIN"."ListaItemId", 
    "QueryMIN"."MenorPreco", 
    ( 	SELECT 
            "OrcamentoItem"."Id" 
		FROM "OrcamentoItem" 
		WHERE 
			"OrcamentoItem"."ListaItemId" = "QueryMIN"."ListaItemId" 
			AND "OrcamentoItem"."Preco" = "QueryMIN"."MenorPreco" 
		LIMIT 1 ) AS "OrcamentoItemId",
    ( 	SELECT 
            "OrcamentoItem"."OrcamentoId" 
		FROM "OrcamentoItem" 
		WHERE 
			"OrcamentoItem"."ListaItemId" = "QueryMIN"."ListaItemId" 
			AND "OrcamentoItem"."Preco" = "QueryMIN"."MenorPreco" 
		LIMIT 1 ) AS "OrcamentoId",
		( 	SELECT 
            "ListaItem"."Quantidade"  
		FROM "ListaItem" 
		WHERE 
			"ListaItem"."Id" = "QueryMIN"."ListaItemId"  
		LIMIT 1 ) AS "Quantidade"
FROM 
    ( SELECT 
        "OrcamentoItem"."ListaItemId", 
        MIN( "OrcamentoItem"."Preco" ) AS "MenorPreco"
    FROM "OrcamentoItem"
    WHERE 
        "OrcamentoItem"."Preco" IS NOT NULL
        AND "OrcamentoItem"."ListaItemId" IS NOT NULL
    GROUP BY "OrcamentoItem"."ListaItemId" ) AS "QueryMIN"


-- versao otimizada pelo chatgpt
-- acho q nao ta funcionando
-- CREATE OR REPLACE VIEW "EconomiaOrcamentoItemView"
-- -- A PROXIMA LINHA APLICA POLICIES NA VIEW
-- WITH (security_invoker=on)
-- AS
-- SELECT 
--     "ListaItem"."ListaId",
-- 	"OrcamentoItem"."ListaItemId", 
--     MIN("OrcamentoItem"."Preco") AS "MenorPreco", 
--     -- quem sabe haja um problema nessa linha
--     MIN("OrcamentoItem"."Id") AS "OrcamentoItemId",
--     "OrcamentoItem"."OrcamentoId" 
-- FROM 
--     "OrcamentoItem"
--     JOIN "ListaItem" ON "OrcamentoItem"."ListaItemId" = "ListaItem"."Id"
-- WHERE 
--     "OrcamentoItem"."Preco" IS NOT NULL
--     AND "OrcamentoItem"."ListaItemId" IS NOT NULL
-- GROUP BY 
--     "OrcamentoItem"."ListaItemId", "ListaItem"."ListaId", "OrcamentoItem"."OrcamentoId"



CREATE OR REPLACE VIEW "EconomiaOrcamentoItemTotalView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT
	"EconomiaOrcamentoItemView"."ListaId",
	SUM( "EconomiaOrcamentoItemView"."MenorPreco" )	AS "PrecoTotal"
FROM "EconomiaOrcamentoItemView"
GROUP BY "EconomiaOrcamentoItemView"."ListaId"




CREATE OR REPLACE VIEW "EconomiaOrcamentosEntregaTotalView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
select 
	"QueryPrecoEntregas"."ListaId",
	sum( "QueryPrecoEntregas"."EntregaPreco" ) as "EntregaPrecoTotal"
from
(	SELECT
		"EconomiaOrcamentoItemView"."ListaId",
		"OrcamentoView"."EntregaPreco"
	FROM "EconomiaOrcamentoItemView"
	join "OrcamentoView" on "OrcamentoView"."OrcamentoId" =  "EconomiaOrcamentoItemView"."OrcamentoId"
	GROUP BY "EconomiaOrcamentoItemView"."ListaId", "OrcamentoView"."EntregaPreco"		) as "QueryPrecoEntregas"
GROUP BY "QueryPrecoEntregas"."ListaId"


CREATE OR REPLACE VIEW "OrcamentoItemView"
-- A PROXIMA LINHA APLICA POLICIES NA VIEW
WITH (security_invoker=on)
AS
SELECT 
	"OrcamentoItem"."Id",
	"OrcamentoItem"."OrcamentoId",
	"OrcamentoItem"."ListaItemId",
	"OrcamentoItem"."Descricao" AS "OrcamentoItem_Descricao",
	"OrcamentoItem"."Quantidade" AS "OrcamentoItem_Quantidade",
	"OrcamentoItem"."UnidadeMedida",
	"OrcamentoItem"."Observacao",
	"OrcamentoItem"."Preco",
	"OrcamentoItem"."Desconto",
	"ListaItem"."ListaId",
	"ListaItem"."Descricao" AS "ListaItem_Descricao",
	"ListaItem"."Quantidade" AS "ListaItem_Quantidade"	
FROM "OrcamentoItem"
JOIN "ListaItem" ON "ListaItem"."Id" = "OrcamentoItem"."ListaItemId"
