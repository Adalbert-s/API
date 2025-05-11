using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global exception handling
if (app.Environment.IsDevelopment())
{
    // Em ambiente de desenvolvimento, mostrar página de erro detalhada
    app.UseDeveloperExceptionPage();
}
else
{
    // Em produção, capturar tudo e redirecionar para /error
    app.UseExceptionHandler("/error");
    // Endpoint minimal para retornar 500 padronizado
    app.MapGet("/error", () => Results.Problem("Ocorreu um erro interno.", statusCode: 500));
}

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Health check
app.MapGet("/", () => Results.Ok(new { status = "healthy" }));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
