using Microsoft.EntityFrameworkCore;
using ProvaDiogo.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura a conexão com o banco de dados SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

// Add services to the container.
builder.Services.AddControllers();

// Configure a injeção de dependência para o contexto do banco de dados
builder.Services.AddScoped<AppDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
