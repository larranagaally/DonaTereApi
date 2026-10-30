using DoñaTere.Api.Validators;
using DoñaTereApi.Data;
using DoñaTereApi.Repositories;
using DoñaTereApi.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
var cs = builder.Configuration.GetConnectionString("conexion");
builder.Services.AddDbContext<DoñaTereDbContext>(options => options.UseMySql(cs, ServerVersion.AutoDetect(cs)));



builder.Services.AddScoped(typeof(Repository<>));

builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>(); 
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<PedidoService>();



builder.Services.AddScoped<NewCategoriaValidator>();
builder.Services.AddScoped<NewClienteValidator>();
builder.Services.AddScoped<NewProductoValidator>();
builder.Services.AddScoped<NewPedidoValidator>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DoñaTereDbContext>();
    context.Database.EnsureCreated();
}


app.MapControllers();

app.Run();
