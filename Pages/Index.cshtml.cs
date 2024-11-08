using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Data.Tables;
using Azure;
using IBAS_kantine.Model;

namespace IBAS_kantine.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public List<MenuItem> TableData { get; set; } = new List<MenuItem>();

    public void OnGet()
    {
        var tableName = "IBASKantine2025";
        var connectionString =
            "DefaultEndpointsProtocol=https;AccountName=ibaskantine;AccountKey=v5FkdhmWiSXrARqDaVC/swrmATAV+lrlSWgyH6hO1j/yLrdxDoUuJDX0NunsRq99HFWHkIXJaeXQ+ASt6vMHqg==;EndpointSuffix=core.windows.net";
        var tableClient = new TableClient(connectionString, tableName);
        //gjort så sevret godt kan pushes
        Pageable<TableEntity> queryResults = tableClient.Query<TableEntity>();

        foreach (var entity in queryResults)
        {
            var menuItem = new MenuItem
            {
                PartitionKey = entity.PartitionKey,
                RowKey = entity.RowKey,
                Lokation = entity.GetString("Lokation"),
                Ugedag = entity.RowKey,
                Hotmeal = entity.GetString("Hotmeal"),
                Coldmeal = entity.GetString("Coldmeal")
            };
            TableData.Add(menuItem);
        }
        
        var weekdayOrder = new List<string> { "Man", "Tirs", "Ons", "Tors", "Fre" };
        TableData = TableData
            .OrderBy(item => weekdayOrder.IndexOf(item.Ugedag))
            .ToList();
    }
}
