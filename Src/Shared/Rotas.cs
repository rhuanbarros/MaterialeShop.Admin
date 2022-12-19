namespace MaterialeShop.Admin.Src.Shared;
public static class Rotas
{
    public const string listas = "listas";
    public const string endereco = "endereco";
    public const string clientes = "clientes";
    public const string lojas = "lojas";
    public const string aguardando_orcamentos = "aguardando-orcamentos";
    public const string carrinhos = "carrinhos";
    public const string pedidos = "pedidos";
    public const string configuracoes = "configuracoes";

    public static string Lista(int ListaId)
    {
        return $"/lista/{ListaId}";
    }
    
    public static string Selecionar_loja(int ListaId)
    {
        return $"/lista/{ListaId}/selecionar-loja";
    }
    
    public static string Orcamentos_lista(int ListaId)
    {
        return $"/lista/{ListaId}/orcamentos";
    }

}