-----------------  SECURITY POLICIES FUNCTIONS

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

-- função para retornar Perfil.Id do usuario logado
        CREATE OR REPLACE FUNCTION get_Perfil_Id_for_authenticated_user()
        returns setof int8
        language sql
        security definer
        set search_path = public
        stable
        as $$
            SELECT "Perfil"."Id" FROM "Perfil"
                WHERE "UserUuid" = auth.uid()
        $$;

-- função para retornar Lista.Id de listas que o usuario é o proprietario
        CREATE OR REPLACE FUNCTION get_Lista_Id_authenticated_user_own_Lista()
        returns setof int8
        language sql
        security definer
        set search_path = public
        stable
        as $$
            SELECT "Lista"."Id" 
                FROM "Lista"
                WHERE "Lista"."PerfilId" IN ( 
                        SELECT get_Perfil_Id_for_authenticated_user() 
                    )
        $$;


-----------------

----------------- | LISTA
    -- apenas ver suas proprios linhas
    -- ou admin

    ----------------- ADMIN POLICIES

        CREATE POLICY "Users can ALL QUERIES if they are admin"
            ON "public"."Lista"
            FOR ALL 
            USING (
                auth.uid() IN (
                SELECT get_AdminUsers_row_for_authenticated_user()
                )
            );

    ----------------- USERS POLICIES

        CREATE POLICY "Users can SELECT if they own row"
            ON "public"."Lista"
            AS PERMISSIVE
            FOR SELECT
            TO authenticated
            USING ( "Lista"."PerfilId" = ( 
                SELECT get_Perfil_Id_for_authenticated_user()
                )
            );

        CREATE POLICY "Users can UPDATE if they own row"
            ON "public"."Lista"
            AS PERMISSIVE
            FOR UPDATE
            TO authenticated
            USING ( "Lista"."PerfilId" = ( 
                SELECT get_Perfil_Id_for_authenticated_user()
                )
            );

        CREATE POLICY "Users can DELETE if they own row"
            ON "public"."Lista"
            AS PERMISSIVE
            FOR DELETE
            TO authenticated
            USING ( "Lista"."PerfilId" = ( 
                SELECT get_Perfil_Id_for_authenticated_user()
                )
            );
        
        CREATE POLICY "Users can INSERT if they own row"
            ON "public"."Lista"
            AS PERMISSIVE
            FOR INSERT
            TO authenticated
            WITH CHECK ( "Lista"."PerfilId" = ( 
                SELECT get_Perfil_Id_for_authenticated_user()
                )
            );

----------------- | ListaItem
    -- apenas ver linhas em que o ListaId seja de listas q ele pode ver
    -- ou admin

    ----------------- ADMIN POLICIES

        CREATE POLICY "Users can ALL QUERIES if they are admin"
            ON "public"."ListaItem"
            FOR ALL 
            USING (
                auth.uid() IN (
                    SELECT get_AdminUsers_row_for_authenticated_user()
                )
            );

    ----------------- USERS POLICIES

        CREATE POLICY "Users can SELECT if they own Lista"
            ON "public"."ListaItem"
            AS PERMISSIVE
            FOR SELECT
            TO authenticated
            USING ( "ListaItem"."ListaId" IN ( 
                    SELECT get_Lista_Id_authenticated_user_own_Lista()
                )
            );

        CREATE POLICY "Users can UPDATE if they own Lista"
            ON "public"."ListaItem"
            AS PERMISSIVE
            FOR UPDATE
            TO authenticated
            USING ( "ListaItem"."ListaId" IN ( 
                    SELECT get_Lista_Id_authenticated_user_own_Lista()
                )
            );

        CREATE POLICY "Users can DELETE if they own Lista"
            ON "public"."ListaItem"
            AS PERMISSIVE
            FOR DELETE
            TO authenticated
            USING ( "ListaItem"."ListaId" IN ( 
                    SELECT get_Lista_Id_authenticated_user_own_Lista()
                )
            );
        
        CREATE POLICY "Users can INSERT if they own Lista"
            ON "public"."ListaItem"
            AS PERMISSIVE
            FOR INSERT
            TO authenticated
            WITH CHECK ( "ListaItem"."ListaId" IN ( 
                    SELECT get_Lista_Id_authenticated_user_own_Lista()
                )
            );

----------------- | Loja
    -- apenas para usuarios logados
    -- ou admin

    ----------------- ADMIN POLICIES

        CREATE POLICY "Users can ALL QUERIES if they are admin"
            ON "public"."Loja"
            FOR ALL 
            USING (
                auth.uid() IN (
                SELECT get_AdminUsers_row_for_authenticated_user()
                )
            );

    ----------------- USERS POLICIES

        CREATE POLICY "Users can SELECT if authenticated"
            ON "public"."Loja"
            AS PERMISSIVE
            FOR SELECT
            TO authenticated
            USING ( 
                true
                );

----------------- | Orcamento
    - apenas ver linhas em que o ListaId seja de listas q ele pode ver
    - ou admin

----------------- | OrcamentoItem
    - apenas ver linhas em que o ListaId seja de listas q ele pode ver
    - ou admin

----------------- | Perfil
    - apenas ver suas proprios linhas
    - apenas inserir seu proprio registro
    - ou admin

    ----------------- ADMIN POLICIES

        CREATE POLICY "Users can ALL QUERIES if they are admin"
            ON "public"."Perfil"
            FOR ALL 
            USING (
                auth.uid() IN (
                SELECT get_AdminUsers_row_for_authenticated_user()
                )
            );

    ----------------- USERS POLICIES

        -- //TODO no futuro, quem sabe tenha que alterar para ele poder fazer SELECT tbm 
        -- em linhas de usuarios se houver alguma lista compartilhada com ele, ou de alguma lista especifica

        CREATE POLICY "Users can SELECT if they own row"
            ON "public"."Perfil"
            AS PERMISSIVE
            FOR SELECT
            TO authenticated
            USING ( auth.uid() = "UserUuid" );

        CREATE POLICY "Users can UPDATE if they own row"
            ON "public"."Perfil"
            AS PERMISSIVE
            FOR UPDATE
            TO authenticated
            USING ( auth.uid() = "UserUuid" );

        CREATE POLICY "Users can DELETE if they own row"
            ON "public"."Perfil"
            AS PERMISSIVE
            FOR DELETE
            TO authenticated
            USING ( auth.uid() = "UserUuid" );

        -- tem q ser liberado pra todo mundo pq na hora de que o usuario vai crir seu proprio login ele nao esta logado
        -- deixei liberado para usuario autenticado poder INSERT pq é necessário para o admin possa criar clientes no dashboard
        CREATE POLICY "All users can INSERT"
            ON "public"."Perfil" 
            AS PERMISSIVE
            FOR INSERT
            TO anon, authenticated
            WITH CHECK (true);
            
        -- nao da pra ser usada pq o usuario ainda nao esta logado qdo cria seu nome de usuario
        -- //TODO no futuro verificar se tem como ajustar isso
        -- CREATE POLICY "Users can INSERT only if they own uuid"
        --     ON "public"."Perfil" 
        --     AS PERMISSIVE
        --     FOR INSERT
        --     TO authenticated
        --     WITH CHECK (auth.uid() = "UserUuid");

----------------- | ADMIN
    - totalmente bloqueado
    - fazer edições direto no supabase

