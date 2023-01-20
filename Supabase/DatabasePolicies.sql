CREATE POLICY "policy_name"
ON public.Lista FOR
DELETE USING ( auth.uid() = user_id );

CREATE POLICY "policy_name"
ON public.Lista FOR
SELECT  USING ( auth.uid() = user_id );

#################### TABELA LISTA

CREATE POLICY "Enable read access for all users" ON "public"."Lista"
AS PERMISSIVE FOR SELECT
TO authenticated
USING (true)


CREATE POLICY "Usuarios podem SELECT se houver criado o registro"
    ON "public"."Lista" AS PERMISSIVE FOR SELECT
    TO authenticated
    USING ( auth.uid() = user_id );

CREATE POLICY "Usuarios podem UPDATE se houver criado o registro"
    ON "public"."Lista" AS PERMISSIVE FOR UPDATE
    TO authenticated
    USING ( auth.uid() = user_id );

CREATE POLICY "Usuarios podem DELETE se houver criado o registro"
    ON "public"."Lista" AS PERMISSIVE FOR DELETE
    TO authenticated
    USING ( auth.uid() = user_id );

CREATE POLICY "Usuarios podem INSERT se estiver autenticado"
    ON "public"."Lista" 
    AS PERMISSIVE
    FOR INSERT
    TO authenticated
    WITH CHECK (true);


----------------------------------------

Lista
    - apenas ver suas proprios linhas
    - ou admin

    CREATE POLICY "Users can SELECT if own row"
    ON "public"."Lista"
    AS PERMISSIVE
    FOR SELECT
    TO authenticated
    USING ( auth.uid() = user_id );

    CREATE POLICY "Users can UPDATE if own row"
        ON "public"."TodoPrivate" AS PERMISSIVE FOR UPDATE
        TO authenticated
        USING ( auth.uid() = user_id );

    CREATE POLICY "Users can DELETE if own row"
        ON "public"."TodoPrivate" AS PERMISSIVE FOR DELETE
        TO authenticated
        USING ( auth.uid() = user_id );

    CREATE POLICY "Users can INSERT only if own uuid"
        ON "public"."TodoPrivate" 
        AS PERMISSIVE
        FOR INSERT
        TO authenticated
        WITH CHECK (auth.uid() = user_id);

ListaItem
    - apenas ver linhas em que o ListaId seja de listas q ele pode ver
    - ou admin

Loja
    - apenas para usuarios logados
    - ou admin

Orcamento
    - apenas ver linhas em que o ListaId seja de listas q ele pode ver
    - ou admin

OrcamentoItem
    - apenas ver linhas em que o ListaId seja de listas q ele pode ver
    - ou admin

Perfil
    - apenas ver suas proprios linhas
    - apenas inserir seu proprio registro
    - ou admin

    CREATE POLICY "Users can SELECT if they are admin"
        ON "public"."Perfil"
        FOR ALL USING (
            "UserUuid" IN (
            SELECT get_AdminUsers_row_for_authenticated_user()
            )
        );

    CREATE POLICY "Users can SELECT if own row"
        ON "public"."Perfil"
        AS PERMISSIVE
        FOR SELECT
        TO authenticated
        USING ( auth.uid() = UserUuid );

    CREATE POLICY "Users can SELECT if are admin"
        ON "public"."Perfil"
        AS PERMISSIVE
        FOR SELECT
        TO authenticated
        USING ( auth.uid()  );

    CREATE POLICY "Users can UPDATE if own row"
        ON "public"."TodoPrivate" AS PERMISSIVE FOR UPDATE
        TO authenticated
        USING ( auth.uid() = user_id );

    CREATE POLICY "Users can DELETE if own row"
        ON "public"."TodoPrivate" AS PERMISSIVE FOR DELETE
        TO authenticated
        USING ( auth.uid() = user_id );

    CREATE POLICY "Users can INSERT only if own uuid"
        ON "public"."TodoPrivate" 
        AS PERMISSIVE
        FOR INSERT
        TO authenticated
        WITH CHECK (auth.uid() = user_id);

admin
    - totalmente bloqueado
    - fazer edições direto no supabase

    -- função para retornar a linha de AdminUsers que contenha o propio uuid
    create or replace function get_AdminUsers_row_for_authenticated_user()
        returns setof uuid
        language sql
        security definer
        set search_path = public
        stable
        as $$
            select "UserUuid"
            from "AdminUsers"
            where "UserUuid" = auth.uid()
        $$;