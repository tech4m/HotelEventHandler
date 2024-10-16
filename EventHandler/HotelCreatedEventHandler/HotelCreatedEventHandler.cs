using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SNSEvents;
using HotelCreatedEventHandler.Models;
using Nest;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace HotelCreatedEventHandler;
public class HotelCreatedEventHandler
{
    public async Task Handler(SNSEvent snsEvent)
    {



        var connSettings = new ConnectionSettings(new Uri(host));
        connSettings.BasicAuthentication(userName, password);

        connSettings.DefaultIndex(indexName);
        connSettings.DefaultMappingFor<Hotel>(m => m.IdProperty(p => p.Id));

        var esClient = new Nest.ElasticClient(connSettings);
        if (!(await esClient.Indices.ExistsAsync(indexName)).Exists)
        {
            await esClient.Indices.CreateAsync(indexName);
        }

        foreach (var eventRecord in snsEvent.Records)
        {
            var eventId = eventRecord.Sns.MessageId;
            var foundItem = await table.GetItemAsync(eventId);
            if (foundItem == null)
            {
                await table.PutItemAsync(new Document()
                {
                    ["eventid"] = eventId
                });

                //Process Event Here
            }
            var hotel = JsonSerializer.Deserialize<Hotel>(eventRecord.Sns.Message);

            await esClient.IndexDocumentAsync<Hotel>(hotel);
        }
    }
}
