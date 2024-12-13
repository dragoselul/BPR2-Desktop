namespace BPR2_Desktop.Model;

public class AppConfig
{
    public string? ConfigurationsFolder { get; set; }

    public string? AppPropertiesFileName { get; set; }
    
    public DatabaseConfig DatabaseConfig { get; set; }
}

public class DatabaseConfig
{
    public string? Server { get; set; }
    public string? Port { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Schema { get; set; }

    public string ConnectionString()
    {
        return
            $"Host={Server};Port={Port};Database={Name};Username={Username};Password={Password};SearchPath={Schema};" +
            $"Persist Security Info=False;TrustServerCertificate=False;";
    }
}