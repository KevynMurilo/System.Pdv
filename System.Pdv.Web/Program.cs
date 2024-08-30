using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Pdv.Application.Interfaces.Auth;
using System.Pdv.Application.Interfaces.Authentication;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Application.Services.Auth;
using System.Pdv.Application.Services.Authentication;
using System.Pdv.Application.Services.Clientes;
using System.Pdv.Application.Services.Mesas;
using System.Pdv.Application.Services.MesaService;
using System.Pdv.Application.Services.Roles;
using System.Pdv.Application.Services.Usuarios;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;
using System.Pdv.Web.Filters;
using System.Text;

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

//Criar Role
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IGetAllRolesService, GetAllRolesService>();
builder.Services.AddScoped<ICreateRoleService, CreateRoleService>();

// Injeção JWT
builder.Services.AddScoped<IJwtTokenGeneratorUsuario, JwtTokenGeneratorUsuario>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Builder de mesas
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IGetAllServices, GetAllService>();
builder.Services.AddScoped<IGetMesaByIdService, GetMesaByIdService>();
builder.Services.AddScoped<ICreateMesaService, CreateMesaService>();
builder.Services.AddScoped<IUpdateMesaService, UpdateMesaService>();
builder.Services.AddScoped<IDeleteMesaService, DeleteMesaService>();

//Builder de Usuários
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGetAllUsuarioService, GetAllUsuarioService>();
builder.Services.AddScoped<IGetByIdUsuarioService, GetByIdUsuarioService>();
builder.Services.AddScoped<ICreateUsuarioService, CreateUsuarioService>();
builder.Services.AddScoped<IUpdateUsuarioService, UpdateUsuarioService>();
builder.Services.AddScoped<IDeleteUsuarioService, DeleteUsuarioService>();

//Builder de clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICreateClienteService, CreateClienteService>();

// Configuração do JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API BLOG", Version = "v1" });
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo:",
    });

    c.OperationFilter<AuthorizeOperationFilter>();
});

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
