using System.Text.Json;
using KinoBot.API.Models;
using KinoBot.API.Enums;

var json = """
{
    "page": 1,
    "results": [
        {
            "id": 1,
            "media_type": "movie",
            "title": "Movie Title"
        },
        {
            "id": 2,
            "media_type": "tv",
            "name": "TV Show Name"
        },
        {
            "id": 3,
            "media_type": "person",
            "name": "Person Name"
        }
    ],
    "total_pages": 1,
    "total_results": 3
}
""";

try 
{
    var response = JsonSerializer.Deserialize<SearchResponse>(json);
    if (response == null) {
        Console.WriteLine("Response is null");
        return;
    }
    Console.WriteLine($"Results count: {response.Results.Count}");
    foreach(var result in response.Results) {
        Console.WriteLine($"ID: {result.Id}, MediaType: {result.MediaType}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
