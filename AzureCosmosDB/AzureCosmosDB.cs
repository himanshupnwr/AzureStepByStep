using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
namespace ConsoleApp7
{
    public class JsonData
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
    public class AzureCosmosDB
    {
        static void Main(string[] args)
        {
            const string EndpointUrl = @"https://mycosmos123.documents.azure.com:443/";
            const string PrimaryKey = @"JV50wRKtAm7taXPh52zaiRgVBCR1sCNlMsnpV6b4Ie1iT1nU1gAw7hjeR3vpJPnBVzZUcZS4RkKSYuSpWOmjtg==";
            DocumentClient client;
            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<JsonData> familyQuery = client.CreateDocumentQuery<JsonData>(
                    UriFactory.CreateDocumentCollectionUri("db1", "coll1"), queryOptions);
            foreach (var x in familyQuery)
            {
                Console.WriteLine("\tRead {0}", x.id + "  " + x.Name);
            }
            Console.Read();
        }
    }
}
