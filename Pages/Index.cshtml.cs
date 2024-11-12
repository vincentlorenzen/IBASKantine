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
                //Siden environmental variable ikke virkede,sætter jeg nu direkte value fra den ind istedet for navnet
                var tableEndpoint = new Uri("https://ibaskantine.table.core.windows.net");

                var tableName = "IBASKantine2024"; // Replace with your actual table name

                var credential = new DefaultAzureCredential();
                var tableClient = new TableClient(tableEndpoint, tableName, new DefaultAzureCredential());

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


