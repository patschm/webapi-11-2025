using Microsoft.Data.SqlClient;

namespace ProductReviews.DAL.EntityFramework.Database;

public class SqlConnectionBuilder
{
    private readonly SqlConnectionStringBuilder _builder = new();

    public SqlConnectionBuilder()
    {
        _builder.DataSource = @".\SQLEXPRESS";
        _builder.IntegratedSecurity = true;
        _builder.TrustServerCertificate = true;
        _builder.MultipleActiveResultSets = true;
    }
    public SqlConnectionBuilder Database(string dbName = "Mode1DB")
    {
        _builder.InitialCatalog = dbName;
        return this;
    }
    public SqlConnectionBuilder DataDource(string source)
    {
        _builder.DataSource = source;
        return this;
    }

    public string Build()
    {
        return _builder.ConnectionString;
    }

    public static string ExpressConnectionString(string databaseName = "Mod1DB")
    {
        return new SqlConnectionBuilder()
            .Database(databaseName)
            .Build();
    }
}
