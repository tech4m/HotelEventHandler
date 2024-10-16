// See https://aka.ms/new-console-template for more information
using Amazon.Lambda.SNSEvents;
using HotelCreatedEventHandler;
using HotelCreatedEventHandler.Models;
using System.Text.Json;

Environment.SetEnvironmentVariable("host", "https://507b3c520fed4264a49694ead351d6d4.us-central1.gcp.cloud.es.io");
Environment.SetEnvironmentVariable("userName", "elastic");
Environment.SetEnvironmentVariable("password", "UrZfevHAhpsBRH04HMZZnkq5");
Environment.SetEnvironmentVariable("indexName", "event");
Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");

var hotel = new Hotel()
{
    Name = "Taj Hotel",
    City = "Chandigarh",
    Id = "123",
    Rating = 4,
    Price = 100,
    UserId = "ABC",
    CreationDateTime = DateTime.Now
};

var snsEvent = new SNSEvent()
{
    Records = new List<SNSEvent.SNSRecord>() { new SNSEvent.SNSRecord() {Sns=new SNSEvent.SNSMessage()
    {
        MessageId = "100",
        Message=JsonSerializer.Serialize(hotel)
    } } }
};


var handler = new HotelCreatedEventHandler.HotelCreatedEventHandler();
await  handler.Handler(snsEvent);