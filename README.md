# MaterialeShop.Admin

# para fazer
	- https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-lazy-load-assemblies?view=aspnetcore-7.0
	- https://code-maze.com/lazy-loading-in-blazor-webassembly/#:~:text=Lazy%20Loading%20enables%20us%20to,we%20do%20not%20require%20yet.
	- https://digitteck.com/frontend/blazor/blazor-page-in-another-assembly/
	- https://learn.microsoft.com/en-us/aspnet/core/blazor/components/class-libraries?view=aspnetcore-7.0&tabs=visual-studio

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