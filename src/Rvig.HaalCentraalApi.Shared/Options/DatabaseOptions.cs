namespace Rvig.HaalCentraalApi.Shared.Options;
public class DatabaseOptions
{
	public const string DatabaseSection = "Database";

	public string Host {  get; set; } = string.Empty;
	public string Port {  get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Database { get; set; } = string.Empty;
	public bool LogQueryAsMultiLiner { get; set; }
	public double RefreshLandelijkeTabellen { get; set; }

    // explicit connectivity/query diagnostics controls
    public int ConnectionTimeoutSeconds { get; set; } = 15;
    public int CommandTimeoutSeconds { get; set; } = 30;
    public int KeepAliveSeconds { get; set; } = 30;
    public bool IncludeErrorDetail { get; set; } = false;

    public string ConnectionString
    {
        get
        {
            var portPart = !string.IsNullOrWhiteSpace(Port) ? $"Port={Port};" : "";
            var timeoutPart = ConnectionTimeoutSeconds > 0 ? $"Timeout={ConnectionTimeoutSeconds};" : "";
            var commandTimeoutPart = CommandTimeoutSeconds > 0 ? $"Command Timeout={CommandTimeoutSeconds};" : "";
            var keepAlivePart = KeepAliveSeconds > 0 ? $"Keepalive={KeepAliveSeconds};" : "";
            var includeErrorDetailPart = IncludeErrorDetail ? "Include Error Detail=true;" : "";

            return $"Host={Host};{portPart}Username={Username};Password={Password};Database={Database};{timeoutPart}{commandTimeoutPart}{keepAlivePart}{includeErrorDetailPart}";
        }
    }
}