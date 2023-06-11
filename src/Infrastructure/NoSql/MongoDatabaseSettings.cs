namespace Infrastructure;
public class MongoDatabaseSettings
{
    public string ManagerCollectionName { get; set; } = string.Empty;
    public string OrganizerCollectionName { get; set; } = string.Empty;
    public string PlayerCollectionName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DataBaseName { get; set; } = string.Empty;
}
