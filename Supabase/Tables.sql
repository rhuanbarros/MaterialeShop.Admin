CREATE TABLE IF NOT EXISTS public."Loja"
(
    "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "CreatedAt" timestamp with time zone DEFAULT now(),
    "Nome" text COLLATE pg_catalog."default" NOT NULL,
    "Cnpj" text COLLATE pg_catalog."default",
    "Email" text COLLATE pg_catalog."default",
    "Telefone" text COLLATE pg_catalog."default",
    "Endereco" text COLLATE pg_catalog."default",
    "Cidade" text COLLATE pg_catalog."default",
    "Estado" text COLLATE pg_catalog."default",
    CONSTRAINT "Loja_pkey" PRIMARY KEY (Id)
)

CREATE TABLE IF NOT EXISTS public."Lista"
(
    "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "CreatedAt" timestamp with time zone DEFAULT now(),
    "PerfilId" bigint,
    "Endereco" text COLLATE pg_catalog."default",
    "Status" text COLLATE pg_catalog."default",
    CONSTRAINT "Lista_pkey" PRIMARY KEY (Id),
    CONSTRAINT "Lista_PerfilId_fkey" FOREIGN KEY ("PerfilId")
        REFERENCES public."Perfil" (Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

CREATE TABLE IF NOT EXISTS public."Perfil"
(
    "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "Uuid" text COLLATE pg_catalog."default",
    "CreatedAt" timestamp with time zone DEFAULT now(),
    "email" text COLLATE pg_catalog."default",
    "NomeCompleto" text COLLATE pg_catalog."default" NOT NULL,
    "Cpf" text COLLATE pg_catalog."default",
    "Telefone" text COLLATE pg_catalog."default",
    CONSTRAINT "Perfil_pkey" PRIMARY KEY (Id),
    CONSTRAINT "Perfil_Email_key" UNIQUE (Email),
    CONSTRAINT "Perfil_Uuid_key" UNIQUE (Uuid)
)

CREATE TABLE IF NOT EXISTS public."ListaItem"
(
    "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "CreatedAt" timestamp with time zone DEFAULT now(),
    "Descricao" text COLLATE pg_catalog."default" NOT NULL,
    "Quantidade" text COLLATE pg_catalog."default",
    "UnidadeMedida" text COLLATE pg_catalog."default",
    "ListaId" bigint NOT NULL,
    CONSTRAINT "ListaItem_pkey" PRIMARY KEY (Id),
    CONSTRAINT "ListaItem_ListaId_fkey" FOREIGN KEY ("ListaId")
        REFERENCES public."Lista" (Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

CREATE TABLE IF NOT EXISTS public."Orcamento"
(
    "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "CreatedAt" timestamp with time zone DEFAULT now(),
    "ListaId" bigint NOT NULL,
    "LojaId" bigint NOT NULL,
    "SolicitacaoData" timestamp with time zone,
    "Recebido" boolean,
    "RecebidoData" timestamp with time zone,
    "EntregaPreco" numeric,
    "EntregaPrazo" text COLLATE pg_catalog."default",
    "DescontoNoTotal" text COLLATE pg_catalog."default",
    "OrcamentoAnexo" text COLLATE pg_catalog."default",
    "CodigoLoja" text COLLATE pg_catalog."default",
    CONSTRAINT "Orcamento_pkey" PRIMARY KEY (Id),
    CONSTRAINT "Orcamento_ListaId_fkey" FOREIGN KEY ("ListaId")
        REFERENCES public."Lista" (Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Orcamento_LojaId_fkey" FOREIGN KEY ("LojaId")
        REFERENCES public."Loja" (Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)