using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace tableConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = GetBuilderConfig();
            var connectionString = config["connectionString"];
            var table = CreateTableAsync("table001", connectionString).Result;
       
           
            Console.ReadLine();
        }

        static IConfiguration GetBuilderConfig()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            return config;

            //string connectionString = config["connectionString"];
        }


        static async Task<CloudTable> CreateTableAsync(string tableName, string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            if(await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Table Created {0}", tableName);
            }
            else
            {
                Console.WriteLine("The Table {0} Exists", tableName);
            }
            await InsertOperationAsync(table);
            Console.WriteLine("Done");
            return table;
        }


        static async Task InsertOperationAsync(CloudTable table)
        {
            Contact contact = new Contact("Manuel", "Garcia")
            {
                Email = "manuel@hotmail.com",
                Phone = "23131313131"
            };


            TableOperation tableOperation = TableOperation.InsertOrMerge(contact);

            TableResult result = await table.ExecuteAsync(tableOperation);
            Contact insertedContact = result.Result as Contact;

            Console.WriteLine("Contact Added");
        }

    }
}
