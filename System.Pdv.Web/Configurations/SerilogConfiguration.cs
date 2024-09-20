using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Events;

namespace System.Pdv.Web.Configurations.Logging;

public static class SerilogConfiguration
{
    public static void AddSerilogConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

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
                restrictedToMinimumLevel: LogEventLevel.Error
            )
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}
