using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;
public class MongoDatabaseSettings
{
    public string ManagerCollectionName { get; set; } = string.Empty;
    public string OrganizerCollectionName { get; set; } = string.Empty;
    public string PlayerCollectionName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DataBaseName { get; set; } = string.Empty;
}

public class MongoDatabaseSettingsSetup : IConfigureOptions<MongoDatabaseSettings>
{
    const string SECTION_NAME = "MongoDatabaseSettings";
    private readonly IConfiguration _configuration;

    public MongoDatabaseSettingsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(MongoDatabaseSettings options) => _configuration.GetSection(SECTION_NAME).Bind(options);
}
