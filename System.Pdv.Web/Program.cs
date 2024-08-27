using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Application.Services.Clientes;
using System.Pdv.Application.Services.Mesas;
using System.Pdv.Application.Services.MesaService;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração do Entity Framework Core com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configuração do Serilog
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    { "message", new RenderedMessageColumnWriter() },
    { "message_template", new MessageTemplateColumnWriter() },
    { "level", new LevelColumnWriter() },
    { "raise_date", new TimestampColumnWriter() },
    { "exception", new ExceptionColumnWriter() },
    { "properties", new LogEventSerializedColumnWriter() }
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: connectionString,
        tableName: "Logs",
        columnOptions: columnWriters,
        needAutoCreateTable: true,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
    )
    .CreateLogger();

// Adiciona Serilog ao host
builder.Host.UseSerilog();

//Builder de mesas
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<ICreateMesaService, CreateMesaService>();
builder.Services.AddScoped<IGetAllServices, GetAllService>();
builder.Services.AddScoped<IDeleteMesaService, DeleteMesaService>();

//Builder de clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICreateClienteService, CreateClienteService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
