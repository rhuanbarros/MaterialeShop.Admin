create table "public"."UsuarioPerfil" (
  id uuid references auth.users not null,
  Email text,
  NomeCompleto text,
  primary key (id)
);

alter table
  public.profiles enable row level security;