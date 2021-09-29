using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzure.ServiceBus
{
    public class ServiceBusSender
    {
        //client that owns the connection and can be used to create senders and receivers
        ServiceBusClient client;

        //sender used to publish messages to the queue
        Azure.Messaging.ServiceBus.ServiceBusSender sender;


        public ServiceBusSender(string conn, string queueName)
        {
            client = new ServiceBusClient(conn);
            sender = client.CreateSender(queueName);
        }


        public async Task SendAsync(List<string> dataRows)
        {
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            int numOfMessages = 0;

            for (int i = 0; i < dataRows.Count(); i++)
            {
                var row = dataRows[i];
                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(new ServiceBusMessage(row)))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }

    }
}
