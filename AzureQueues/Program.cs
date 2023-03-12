using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

static void Main(string[] args)
{
    // Retrieve storage account from connection string.
    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=shivaccount;AccountKey=jdncU7Lho8GzsFVEWHZsJcKW0rHRmg+mYO0bHhe7HjLx4yn5RwJT546aAkZ1n9KFnphRK95PY1W6zkWXWnvVHw==;EndpointSuffix=core.windows.net");

    // Create the queue client.
    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

    // Retrieve a reference to a container.
    CloudQueue queue = queueClient.GetQueueReference("myqueues");
    //var msg = queue.PeekMessage();
    //Console.WriteLine(msg.AsString);

    //using popreceiptid azure idnetifies who has the lock on the data and for how much visibility timeput is set for it
    var msg = queue.GetMessageAsync().Result;
    var messagenew = new CloudQueueMessage(msg.Id, msg.PopReceipt);
    messagenew.SetMessageContent("updated by popreceiptid and id");
    msg.SetMessageContent("this is changed");

    queue.UpdateMessageAsync(messagenew, new TimeSpan(0, 1, 0)
            , MessageUpdateFields.Content
            | MessageUpdateFields.Visibility);

    foreach (var msg1 in queue.GetMessagesAsync(10).Result)
    {
        queue.DeleteMessageAsync(msg1);
    }
    Console.WriteLine(msg.AsString);
    queue.DeleteMessageAsync(msg);

    // Create the queue if it doesn't already exist
    queue.CreateIfNotExistsAsync();
    //// Create a message and add it to the queue.
    CloudQueueMessage message = new CloudQueueMessage("Hello, World");
    queue.AddMessageAsync(message);
    message = new CloudQueueMessage("Hello, World1");
    queue.AddMessageAsync(message);
}