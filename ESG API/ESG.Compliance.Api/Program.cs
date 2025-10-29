using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ESG.Compliance.Api.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()
    ));

// Reposit�rios
builder.Services.AddScoped<ESG.Compliance.Api.Data.Repositories.ILicencaRepository, ESG.Compliance.Api.Data.Repositories.LicencaRepository>();
builder.Services.AddScoped<ESG.Compliance.Api.Data.Repositories.IAuditoriaRepository, ESG.Compliance.Api.Data.Repositories.AuditoriaRepository>();

// Registrar os Servi�os
builder.Services.AddScoped<ESG.Compliance.Api.Services.ILicencaService, ESG.Compliance.Api.Services.LicencaService>();
builder.Services.AddScoped<ESG.Compliance.Api.Services.IAuditoriaService, ESG.Compliance.Api.Services.AuditoriaService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migrações automaticamente quando usando um banco relacional
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ESG.Compliance.Api.Data.ApplicationDbContext>();
    try
    {
        if (db.Database.IsRelational())
        {
            db.Database.Migrate();
        }
    }
    catch
    {
        // Ignora falhas de migração em ambientes sem banco relacional
    }
}

// HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }