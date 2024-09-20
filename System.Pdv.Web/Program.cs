using System.Pdv.Web.Configurations.Authentication;
using System.Pdv.Web.Configurations.Database;
using System.Pdv.Web.Configurations.DependencyInjection;
using System.Pdv.Web.Configurations.Logging;
using System.Pdv.Web.Configurations.Swagger;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura��o de Serilog
builder.Host.AddSerilogConfiguration(builder.Configuration);

// Configura��o do Banco de Dados
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Inje��o de depend�ncias organizadas por categoria
builder.Services.AddAuthDependencies();
builder.Services.AdicionalDependencies();
builder.Services.AddCategoriaDependencies();
builder.Services.AddClienteDependencies();
builder.Services.AddMesaDependencies();
builder.Services.AddMetodoPagamentoDependencies();
builder.Services.AddPedidoDependencies();
builder.Services.AddTransactionDependencies();
builder.Services.AddProdutoDependencies();
builder.Services.AddRoleDependencies();
builder.Services.AddThermalDependencies();
builder.Services.AddStatusPedidoDependencies();
builder.Services.AddUsuarioDependencies();
builder.Services.AddPermissaoDependencies();
builder.Services.AddAuthorizationDependencies();

// Configura��o do JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configura��o de Swagger
builder.Services.AddSwaggerConfiguration();

// Configura��o de Serializa��o JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configura��o do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
