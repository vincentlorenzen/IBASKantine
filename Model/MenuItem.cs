using System.Reflection.Metadata.Ecma335;
using Azure;

namespace IBAS_kantine.Model;

public class MenuItem
{
    public string? Lokation { get; set; }
    public string? Ugedag { get; set; }
    public string? Hotmeal { get; set; }
    public string? Coldmeal { get; set; }
    
    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}