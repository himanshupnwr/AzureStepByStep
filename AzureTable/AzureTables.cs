using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTable
{
    internal class AzureTables
    {
        public record Product : ITableEntity
        {
            public string RowKey { get; set; } = default!;

            public string PartitionKey { get; set; } = default!;

            public string Name { get; init; } = default!;

            public int Quantity { get; init; }

            public bool Sale { get; init; }

            public ETag ETag { get; set; } = default!;

            public DateTimeOffset? Timestamp { get; set; } = default!;
        }

        public async void useTables()
        {
            //add the connection string in env variable then this code will work
            TableServiceClient tableServiceClient = new TableServiceClient(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));

            TableClient tableClient = tableServiceClient.GetTableClient(tableName: "adventureworks");

            await tableClient.CreateIfNotExistsAsync();

            var prod1 = new Product()
            {
                RowKey = "68719518388",
                PartitionKey = "gear-surf-surfboards",
                Name = "Ocean Surfboard",
                Quantity = 8,
                Sale = true
            };

            // Add new item to server-side table
            await tableClient.AddEntityAsync<Product>(prod1);

            var product = await tableClient.GetEntityAsync<Product>(
                    rowKey: "68719518388",
                    partitionKey: "gear-surf-surfboards"
            );
            Console.WriteLine("Single product:");
            Console.WriteLine(product.Value.Name);

            var prod2 = new Product()
            {
                RowKey = "68719518390",
                PartitionKey = "gear-surf-surfboards",
                Name = "Sand Surfboard",
                Quantity = 5,
                Sale = false
            };

            await tableClient.AddEntityAsync<Product>(prod2);

            var products = tableClient.Query<Product>(x => x.PartitionKey == "gear-surf-surfboards");

            Console.WriteLine("Multiple products:");
            foreach (var item in products)
            {
                Console.WriteLine(item.Name);
            }

        }
    }
}
