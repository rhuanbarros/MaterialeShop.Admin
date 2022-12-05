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