# MaterialeShop.Admin

# para fazer
	- https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-lazy-load-assemblies?view=aspnetcore-7.0
	- https://code-maze.com/lazy-loading-in-blazor-webassembly/#:~:text=Lazy%20Loading%20enables%20us%20to,we%20do%20not%20require%20yet.
	- https://digitteck.com/frontend/blazor/blazor-page-in-another-assembly/
	- https://learn.microsoft.com/en-us/aspnet/core/blazor/components/class-libraries?view=aspnetcore-7.0&tabs=visual-studio
	- [X] SoftDelete em todas as telas
	- [X] SoftDelete em todos os services pra nao ficar vindo registro ja apagado em outras partes
	- [cancelado] refatorar os codigos bases das paginas para passar funções como argumentos nos métodos para evitar o overrride se for realmente necessário
## LOGIN
	- [X] implementar o login em todo site
		- [X] implementar policies
		- [X] confirmar que um usuario soh consiga criar um item se o id do usuario for dele mesmo
		- [cancelado] verificar lentidao em função da mensagem abaixo:
					info: Microsoft.AspNetCore.Authorization.DefaultAuthorizationService[2]
					Authorization failed. These requirements were not met:
					DenyAnonymousAuthorizationRequirement: Requires an authenticated user.
	- TODO configurar emails de recuperação de senha do Supabase	
	- TODO - fazer login com Google
		- [X] arrumar bug de login qdo o token ja prescreveu
	- TODO configurar o token recebido do Supabase, venha com uma role. Daí integrar isso no autorização, pra pessoa nao conseguir abrir a UI.
			- tem codigo de exemplo parece: https://github.com/supabase-community/gotrue-csharp/blob/master/GotrueTests/ClientTests.cs
	
## INTERFACE
	- TODO reduzir o tamanho das letras em 25% quando aberto em celular
	- TODO arrumar bug se clica num campo e clica na tela, o botao de salvar fica ativado e tenta enviar e da erro.
	- [X] campos de data e hora - permitir inserção com texto. usar componentes de mascara.
	- [X] tabela Perfil, mudar o tipo do Campo UUid para tipo uuid e mudar o nome para UserUuid
	- TODO tela de carrinho de compras
	- TODO tela de pedidos
	- TODO tela de configurações
	- TODO criar mascara para os componentes MudTimePicker

	- TODO na tela de lista de comprar, para um cliente, tem q aparecer apenas um botao para criar uma lista, pois se nao fica o campo select com o nome dele apenas.
	
	- TODO fazer a aplicação do cliente
		- TODO fazer bottomNavBar
		- TODO fazer a reutilização de código corretamente entre a aplicação admin e cliente
	- TODO Ícone do carrinho em cima a direita na tela qdo a pessoa estive na tela de comparativo
		- se nao houver carrinhos, aparcer a contagem 0
		- se houver itens nos carrinhos, aparecer o total de itens
	- TODO Tela de carrinhos. verificar se tem telefone cadastrado no perfil da loja, se sim, mostrar o botao do whats
			nao sei se vou implementar esta funcionalidade.

## FUNCIONALIDADE
	- TODO verificar se ta aplicando desconto nos produtos. 
		- quem sabe esperar pra ver como isso aparece nos orçamentos.
	- TODO criar link de compartilhamento da lista
		- TODO quando a pessoa tenta acessar um link de orçamentos ou lista que ela nao tem acesso,
				devido ao RSL, simplesmente nao vai aparecer nada como se nem existisse.
				Entretanto, a interface tem q verificar se a pessoa tem acesso a lista, e mostrar uma tela diferente
	- TODO implementar controle de exceções nos crud bases e database services
	- TODO colocar um link para o site da materiale na mensagem do whats pra loja
		
## BACKEND
	- TODO configurar backend em c# básico na nuvem
	- TODO implementar analitycs
	- TODO implementar interface pra eu saber o q os clientes fizeram no app

## NEGÓCIO
	- TODO Implementar dashboard com dados básicos do livro sobre a jornada do cliente e uso do app
	- 

# Custom Snippets
    
	"LINHA": {
		"prefix": "LINHA",
		"body": [
			"<div class='d-flex flex-row justify-center align-center' style='width: 100% !important' LINHA>",
			"</div>"
		],
		"description": "A div"
	},
	
	"COLUNA": {
		"prefix": "COLUNA",
		"body": [
			"<div class='d-flex flex-column justify-start align-center' style='width: 100% !important' COLUNA>",
			"</div>"
		],
		"description": "A div"
	}
}
# Credits
    https://github.com/supabase-community/supabase-csharp
    https://github.com/patrickgod/BlazorAuthenticationTutorial
    https://github.com/d11-jwaring/SupabaseRealtimeBlazorWASM/tree/master
    

# How to deploy
    dotnet publish -c Release -o release
    firebase deploy

# Error message
    Failed to find a valid digest in the 'integrity' attribute for resource 'https://blazorwasmsupabasetemplate.web.app/_framework/blazor.boot.json' with computed SHA-256 integrity 'XdcujrjLMAFyEwhjckKrX5naw+S/ieI/g8U7BkEVUc8='. The resource has been blocked.
    Unknown error occurred while trying to verify integrity.
    service-worker.js:22 Uncaught (in promise) TypeError: Failed to fetch
        at service-worker.js:22:54
        at async onInstall (service-worker.js:22:5)

    -----> This is because of old files in cache in the browser. Clear cache by clicking in the clear button (just ctrl + f5 doesn't work) and after press ctrl + f5. This will solve.


ALTER ROLE postgres NOSUPERUSER