using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace blobConsole
{   
    class Program
    {
        static void Main(string[] args)
        {
            //Get Config Builder
            var config = GetBuilderConfig();
            string connectionString = config["connectionString"];

       
            //Create a unique name for the container
            string containerName = "test";

            // Create the container and return a container client object
            BlobServiceClient blobServiceClient = CreateServiceClient(connectionString);

            // Get and create the container for the blobs
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            // Create a local file in the ./data/ directory for uploading and downloading
            string localPath =  Directory.GetCurrentDirectory();
            string fileName = "test" + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);
            using (var fs = new FileStream(localFilePath, FileMode.OpenOrCreate))
            {
                using (var wr = new StreamWriter(fs))
                {
                    wr.WriteLine("TEST CONTENT");
                }
            }
            
             // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Upload data from the local file
            blobClient.Upload(localFilePath, true);
            Console.WriteLine("Uploaded Successfully");
            Console.ReadLine();
        }

        static BlobServiceClient CreateServiceClient(string connectionString)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
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




    }
}
