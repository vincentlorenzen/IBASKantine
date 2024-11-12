using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Data.Tables;
using Azure;
using Azure.Identity;
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
            try
            {
                // Retrieve the Table Storage endpoint from the environment variable
                var tableEndpoint = new Uri("https://ibaskantine.table.core.windows.net");

                var tableName = "IBASKantine2025"; // Replace with your actual table name

                // Use DefaultAzureCredential for secure authentication
                var credential = new DefaultAzureCredential();
                var tableClient = new TableClient(tableEndpoint, tableName, new DefaultAzureCredential());

                // Query the table data
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

                // Sort the TableData by weekday order
                var weekdayOrder = new List<string> { "Man", "Tirs", "Ons", "Tors", "Fre" };
                TableData = TableData
                    .OrderBy(item => weekdayOrder.IndexOf(item.Ugedag))
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log the error and display an error message
                Console.WriteLine("Error: " + ex.Message);
                _logger.LogError(ex, "Failed to retrieve or process data from Azure Table Storage.");
            }
        }

}
