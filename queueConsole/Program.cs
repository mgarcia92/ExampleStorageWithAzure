using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace queueConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = GetBuilderConfig();
            var connectionString = config["connectionString"];
            var queueClient = CreateQueueClient("queuetest", connectionString);


            //Add Message To Queue
            //AddMessageToQueue(queueClient);

            //Recieving  messages
            GetQueueMessages(queueClient);


            Console.WriteLine("Done");
            Console.ReadLine();
        }

        //-------------------------------------------------
        // Create the queue service client
        //-------------------------------------------------
        static QueueClient CreateQueueClient(string queueName, string connectionString)
        {

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            // Create the queue
            queueClient.CreateIfNotExists();
            return queueClient;
        }


        static void GetQueueMessages(QueueClient queueClient)
        {
            // Get the message from the queue
            QueueMessage[] message = queueClient.ReceiveMessages(20);

            while (message.Length > 0)
            {
                foreach (var queue in message)
                {
                    Console.WriteLine($"{queue.MessageId} - {queue.ExpiresOn} - {queue.Body}");
                    // / Process(i.e.print) the message in less than 30 seconds
                    Console.WriteLine($"Dequeued message: '{queue.Body}'");

                    // Delete the message
                    queueClient.DeleteMessage(queue.MessageId, queue.PopReceipt);
                    
                }
                message = queueClient.ReceiveMessages();
            }



        }


        //static void GetPeekedMessages(QueueClient queueClient)
        //{
        //    // Get the message from the queue
        //    var message = queueClient.PeekMessages();

        //    while (message.Length > 0)
        //    {
        //        foreach (var queue in message)
        //        {
        //            Console.WriteLine($"{queue.MessageId} - {queue.ExpiresOn} - {queue.Body}");
        //        }

        //        // / Process(i.e.print) the message in less than 30 seconds
        //        Console.WriteLine($"Dequeued message: '{message[0].Body}'");

        //        // Delete the message
        //        queueClient.DeleteMessage(message[0].MessageId, message[0].PopReceipt);
        //        message = queueClient.ReceiveMessages();
        //    }
        //}

        static void AddMessageToQueue(QueueClient queueClient)
        {
            for (int i = 0; i < 500; i++)
            {
                queueClient.SendMessage(string.Format("Message number {0}", i.ToString()));
            }
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
