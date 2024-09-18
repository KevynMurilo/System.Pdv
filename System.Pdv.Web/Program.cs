using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Application.Interfaces.Auth;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Application.UseCase.Auth;
using System.Pdv.Application.UseCase.Mesas;
using System.Pdv.Application.UseCase.Categorias;
using System.Pdv.Application.UseCase.MetodosPagamento;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Application.UseCase.Produtos;
using System.Pdv.Application.UseCase.Roles;
using System.Pdv.Application.UseCase.StatusPedidos;
using System.Pdv.Application.UseCase.Usuarios;
using System.Pdv.Application.UseCase.Adicionais;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;
using System.Pdv.Web.Filters;
using System.Text;
using System.Text.Json.Serialization;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Application.UseCase.Clientes;
using System.Pdv.Infrastructure.Services.Printer;
using System.Pdv.Application.Interfaces.Authorization;
using System.Pdv.Application.UseCase.Autorizacao;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Application.UseCase.Permissoes;
using System.Pdv.Application.UseCase.PermissaoHasRole;

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

//Configuração impressora termica
builder.Services.AddScoped<IThermalPrinterService>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new ThermalPrinterService(configuration);
});

//Builder de Autorização
builder.Services.AddScoped<IAuthorizationUseCase, AuthorizationUseCase>();

//Builder de Permissão
builder.Services.AddScoped<IPermissaoRepository, PermissaoRepository>();
builder.Services.AddScoped<IGetAllPermissaoUseCase, GetAllPermissaoUseCase>();
builder.Services.AddScoped<IGetAllPermissaoComRolesUseCase, GetAllPermissaoComRolesUseCase>();
builder.Services.AddScoped<IGetAllPermissaoByRoleIdUseCase, GetAllPermissaoByRoleIdUseCase>();

//Builder de RolePermissao
builder.Services.AddScoped<IAssignPermissionToRoleUseCase, AssignPermissionToRoleUseCase>();
builder.Services.AddScoped<IRemovePermissionFromRoleUseCase, RemovePermissionFromRoleUseCase>();

//Builder para transações
builder.Services.AddScoped<ITransactionManager, TransactionManager>();

//Builder de Pedidos
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPrintPedidoByIdsUseCase, PrintPedidoByIdsUseCase>();
builder.Services.AddScoped<IProcessarItensPedidoUseCase, ProcessarItensPedidoUseCase>();
builder.Services.AddScoped<IValidarPedidosUseCase, ValidarPedidosUseCase>();
builder.Services.AddScoped<IGetAllPedidosUseCase, GetAllPedidosUseCase>();
builder.Services.AddScoped<IGetPedidosByMesaUseCase, GetPedidosByMesaUseCase>();
builder.Services.AddScoped<IGetByIdPedidoUseCase, GetByIdPedidoUseCase>();
builder.Services.AddScoped<ICreatePedidoUseCase, CreatePedidoUseCase>();
builder.Services.AddScoped<IUpdatePedidoUseCase, UpdatePedidoUseCase>();
builder.Services.AddScoped<IDeletePedidoUseCase , DeletePedidoUseCase>();

//Builder de Roles
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IGetAllRolesUseCase, GetAllRolesUseCase>();
builder.Services.AddScoped<IGetByIdRoleUseCase, GetByIdRoleUseCase>();
builder.Services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
builder.Services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();
builder.Services.AddScoped<IDeleteRoleUseCase , DeleteRoleUseCase>();

// Injeção Autenticação
builder.Services.AddScoped<IAuthUseCase, AuthUseCase>();

//Builder de Clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IGetAllClienteUseCase, GetAllClienteUseCase>();
builder.Services.AddScoped<IGetByNameClienteUseCase, GetByNameClienteUseCase>();
builder.Services.AddScoped<IGetByIdClienteUseCase, GetByIdClienteUseCase>();

//Builder de mesas
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IGetAllMesaUseCase, GetAllMesaUseCase>();
builder.Services.AddScoped<IGetMesaByIdUseCase, GetMesaByIdUseCase>();
builder.Services.AddScoped<ICreateMesaUseCase, CreateMesaUseCase>();
builder.Services.AddScoped<IUpdateMesaUseCase, UpdateMesaUseCase>();
builder.Services.AddScoped<IDeleteMesaUseCase, DeleteMesaUseCase>();

//Builder de Usuários
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGetAllUsuarioUseCase, GetAllUsuarioUseCase>();
builder.Services.AddScoped<IGetByIdUsuarioUseCase, GetByIdUsuarioUseCase>();
builder.Services.AddScoped<ICreateUsuarioUseCase, CreateUsuarioUseCase>();
builder.Services.AddScoped<IUpdateUsuarioUseCase, UpdateUsuarioUseCase>();
builder.Services.AddScoped<IDeleteUsuarioUseCase, DeleteUsuarioUseCase>();
builder.Services.AddScoped<IJwtTokenGeneratorUsuario, JwtTokenGeneratorUsuario>();

//Builder de Categorias
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IGetAllCategoriaUseCase, GetAllCategoriaUseCase>();
builder.Services.AddScoped<IGetByIdCategoriaUseCase, GetByIdCategoriaUseCase>();
builder.Services.AddScoped<ICreateCategoriaUseCase, CreateCategoriaUseCase>();
builder.Services.AddScoped<IUpdateCategoriaUseCase,  UpdateCategoriaUseCase>();
builder.Services.AddScoped<IDeleteCategoriaUseCase, DeleteCategoriaUseCase>();

//Builder de Produtos
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IGetAllProdutoUseCase, GetAllProdutoUseCase>();
builder.Services.AddScoped<IGetByIdProdutoUseCase, GetByIdProdutoUseCase>();
builder.Services.AddScoped<IGetProdutoByCategoriaUseCase, GetProdutoByCategoriaUseCase>();
builder.Services.AddScoped<ICreateProdutoUseCase, CreateProdutoUseCase>();
builder.Services.AddScoped<IUpdateProdutoUseCase, UpdateProdutoUseCase>();
builder.Services.AddScoped<IDeleteProdutoUseCase, DeleteProdutoUseCase>();

//Builder de Adicionais
builder.Services.AddScoped<IAdicionalRepository, AdicionalRepository>();
builder.Services.AddScoped<IGetAllAdicionalUseCase, GetAllAdicionalUseCase>();
builder.Services.AddScoped<IGetByIdAdicionalUseCase, GetByIdAdicionalUseCase>();
builder.Services.AddScoped<ICreateAdicionalUseCase, CreateAdicionalUseCase>();
builder.Services.AddScoped<IUpdateAdicionalUseCase, UpdateAdicionalUseCase>();
builder.Services.AddScoped<IDeleteAdicionalUseCase,  DeleteAdicionalUseCase>();

//Builder de MetodoPagamento
builder.Services.AddScoped<IMetodoPagamentoRepository, MetodoPagamentoRepository>();
builder.Services.AddScoped<IGetAllMetodoPagamentoUseCase, GetAllMetodoPagamentoUseCase>();
builder.Services.AddScoped<IGetByIdMetodoPagamentoUseCase, GetByIdMetodoPagamentoUseCase>();
builder.Services.AddScoped<ICreateMetodoPagamentoUseCase, CreateMetodoPagamentoUseCase>();
builder.Services.AddScoped<IUpdateMetodoPagamentoUseCase, UpdateMetodoPagamentoUseCase>();
builder.Services.AddScoped<IDeleteMetodoPagamentoUseCase, DeleteMetodoPagamentoUseCase>();

//Builder de StatusPedido
builder.Services.AddScoped<IStatusPedidoRepository, StatusPedidoRepository>();
builder.Services.AddScoped<IGetAllStatusPedidoUseCase, GetAllStatusPedidoUseCase>();
builder.Services.AddScoped<IGetByIdStatusPedidoUseCase,  GetByIdStatusPedidoUseCase>();
builder.Services.AddScoped<ICreateStatusPedidoUseCase, CreateStatusPedidoUseCase>();
builder.Services.AddScoped<IUpdateStatusPedidoUseCase, UpdateStatusPedidoUseCase>();
builder.Services.AddScoped<IDeleteStatusPedidoUseCase, DeleteStatusPedidoUseCase>();

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

//SERIALIZAÇÃO
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

// Configuração Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SISTEMA PARA GERENCIAMENTO DE PEDIDOS E MESAS", Version = "v1" });
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

    c.OperationFilter<RemoveExamplesOperationFilter>();
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
