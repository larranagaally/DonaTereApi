ProyectoAPIDoñaTere

Doña Tere - Sistema de Gestión y Pedidos Integrantes del Equipo Emily Charlotte Ortega Davila Angel Antonio Larrañaga Reyes

Configuración del Dev Tunnel en DonaTereApp (Cliente)

Inicia la Web API DoñaTereApi desde Visual Studio asegurándote de tener un Dev Tunnel activo en modo público.
Copia la URL pública generada por Visual Studio (por ejemplo, https://xxxxxx.devtunnels.ms/).
En el proyecto DonaTereApp, abre la clase de configuración de servicios HTTP (por ejemplo, ApiService.cs.
Actualiza la propiedad BaseAddress con la URL pública de tu túnel: csharp _httpClient.BaseAddress = new Uri("https://tu-tunel.devtunnels.ms/");
